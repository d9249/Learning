import os, time
import matplotlib.pyplot as plt
import itertools
import pickle
import imageio
from torch.autograd import Variable
import torch
from image_loader import *
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from torchvision import datasets, transforms
import torchvision
from torch.autograd import Variable
import torch.utils.data as Data
import sys
import pdb, os, argparse
import visdom
import numpy as np

device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

parser = argparse.ArgumentParser()

parser.add_argument('--best_loss', type=float, default=0.1, help='every n epoch best loss')
opt = parser.parse_args()
device = torch.device("cuda:1" if torch.cuda.is_available() else "cpu")
batch_size = 4
lr = 0.0002
train_epoch = 500

#cuda device load

''' trained model load 
ckpt_path = './shadow_models/'
exp_name = 'shadow_removal/'
args = {
    'snapshot' : '0'
}

G.load_state_dict(torch.load(os.path.join(ckpt_path, exp_name,args['snapshot']+',generator_param' + '.pkl'), map_location=device))
D.load_state_dict(torch.load(os.path.join(ckpt_path, exp_name,args['snapshot']+',discriminator_param' + '.pkl'), map_location=device))
'''

class Flatten(nn.Module):
    def forward(self, x):
        x = x.view(x.size()[0], -1)
        return x

class ResidualBlock(nn.Module):
    def __init__(self, in_features):
        super(ResidualBlock, self).__init__()

        conv_block = [  nn.ReflectionPad2d(1),
                        nn.Conv2d(in_features, in_features, 3),
                        nn.InstanceNorm2d(in_features),
                        nn.ReLU(inplace=True),
                        nn.ReflectionPad2d(1),
                        nn.Conv2d(in_features, in_features, 3),
                        nn.InstanceNorm2d(in_features)  ]

        self.conv_block = nn.Sequential(*conv_block)

    def forward(self, x):
        return x + self.conv_block(x)

class Generator(nn.Module):
    def __init__(self, input_nc, output_nc, n_residual_blocks=9):
        super(Generator, self).__init__() 

        # Initial convolution block( 3, 64 channels)
        model = [   nn.ReflectionPad2d(3),
                    nn.Conv2d(input_nc, 64, 7),
                    nn.InstanceNorm2d(64),
                    nn.ReLU(inplace=True) ]
        
        # Downsampling(64, 128, 256, 512 channel)
        in_features = 64
        out_features = in_features*2
        for _ in range(2):
            model += [  nn.Conv2d(in_features, out_features, 3, stride=2, padding=1),
                        nn.InstanceNorm2d(out_features),
                        nn.ReLU(inplace=True) ]
            in_features = out_features
            out_features = in_features*2

        # Residual blocks(512 channel)
        
        for _ in range(n_residual_blocks):
            
            model += [ResidualBlock(in_features)]
            
        # Upsampling(512, 256, 128, 64 channel)
        out_features = in_features//2
        for _ in range(2):
            model += [  nn.ConvTranspose2d(in_features, out_features, 3, stride=2, padding=1, output_padding=1),
                        nn.InstanceNorm2d(out_features),
                        nn.ReLU(inplace=True) ]
            
            in_features = out_features
            out_features = in_features//2
        
        # Output layer, 64, 3 channels
        model += [  nn.ReflectionPad2d(3),
                    nn.Conv2d(64, output_nc, 7),
                    nn.Tanh() ]
        self.model = nn.Sequential(*model)
        
    def forward(self, x):
        
        return self.model(x)


class discriminator(nn.Module):
    # initializers
    def __init__(self,d=64):
        super(discriminator, self).__init__()
        self.conv1 = nn.Conv2d(6, 64, 4, 2, 1)
        self.conv2 = nn.Conv2d(64, 128, 4, 2, 1)
        self.conv2_bn = nn.BatchNorm2d(128)
        self.conv3 = nn.Conv2d(128, 256, 4, 2, 1)
        self.conv3_bn = nn.BatchNorm2d(256)
        self.conv4 = nn.Conv2d(256, 512, 4, 1, 1)
        self.conv4_bn = nn.BatchNorm2d(512)
        
        self.conv_patch = nn.Conv2d(512, 512, 4, 1, 1)
        self.conv_patch_bn = nn.BatchNorm2d(512)
        
        self.flatten6 = Flatten()
        in_features = 1179648 # tensor flat numbers
        self.linear6 = nn.Linear(in_features,1)
        
        self.conv_g = nn.Conv2d(512,512,4,1,1)
        self.conv_g_bn = nn.BatchNorm2d(512)
        self.conv_g2 = nn.Conv2d(512,1,4,1,1)
        
    # weight_init
    # forward method
    def forward(self, x):
        x = F.leaky_relu(self.conv1(x), 0.2)
        x = F.leaky_relu(self.conv2_bn(self.conv2(x)), 0.2)
        x = F.leaky_relu(self.conv3_bn(self.conv3(x)), 0.2)
        x1 = F.leaky_relu(self.conv4_bn(self.conv4(x)), 0.2)
        
        x = F.relu(self.conv_patch_bn(self.conv_patch(x1)))
        x = F.sigmoid(self.linear6(self.flatten6(x)))
        
        k = F.relu(self.conv_g_bn(self.conv_g(x1)))
        k = F.sigmoid((self.conv_g2(k)))
        
        return k,x

########################################CGAN shadow removal##############

def show_result(num_epoch, show = False, save = False, path = 'result.png'):

    G.eval()
    test_images = G(fixed_z_, fixed_y_label_)
    G.train()

    size_figure_grid = 10
    fig, ax = plt.subplots(size_figure_grid, size_figure_grid, figsize=(5, 5))
    for i, j in itertools.product(range(size_figure_grid), range(size_figure_grid)):
        ax[i, j].get_xaxis().set_visible(False)
        ax[i, j].get_yaxis().set_visible(False)

    for k in range(10*10):
        i = k // 10
        j = k % 10
        ax[i, j].cla()
        ax[i, j].imshow(test_images[k].cpu().data.view(28, 28).numpy(), cmap='gray')

    label = 'Epoch {0}'.format(num_epoch)
    fig.text(0.5, 0.04, label, ha='center')
    plt.savefig(path)

    if show:
        plt.show()
    else:
        plt.close()

def show_train_hist(hist, show = False, save = False, path = 'Train_hist.png'):
    x = range(len(hist['D_losses']))

    y1 = hist['D_losses']
    y2 = hist['G_losses']

    plt.plot(x, y1, label='D_loss')
    plt.plot(x, y2, label='G_loss')

    plt.xlabel('Epoch')
    plt.ylabel('Loss')

    plt.legend(loc=4)
    plt.grid(True)
    plt.tight_layout()

    if save:
        plt.savefig(path)

    if show:
        plt.show()
    else:
        plt.close()
######################################################################################################
# training parameters


dataset = shadow_triplets_loader()
train_data_loader = Data.DataLoader(dataset, batch_size=batch_size)

# network
G = Generator(4,3).to(device)
D = discriminator().to(device)

# L1 loss
L1_loss = nn.L1Loss()
mse = nn.MSELoss()
bce = nn.BCELoss()

# Adam optimizer
G_optimizer = optim.Adam(G.parameters(), lr=lr, betas=(0.5, 0.999))
D_optimizer = optim.Adam(D.parameters(), lr=lr, betas=(0.5, 0.999))

# results save folder
if not os.path.isdir('shadow_models/shadow_removals'):
    os.mkdir('shadow_models/shadow_removals')

train_hist = {}
train_hist['D_losses'] = []
train_hist['G_losses'] = []
train_hist['per_epoch_ptimes'] = []
train_hist['total_ptime'] = []

vis = visdom.Visdom()
plot = vis.line(Y = torch.tensor([0]), X = [0], win='train')

print('training start!')
start_time = time.time()
global_step =0

for epoch in range(train_epoch):
    joint_loss = []
    
    # learning rate decay
    if (epoch+1) == 30:
        G_optimizer.param_groups[0]['lr'] /= 10
        D_optimizer.param_groups[0]['lr'] /= 10
        print("learning rate change!")

    if (epoch+1) == 40:
        G_optimizer.param_groups[0]['lr'] /= 10
        D_optimizer.param_groups[0]['lr'] /= 10
        print("learning rate change!")

    epoch_start_time = time.time()
    
    for x_, y_ in enumerate(train_data_loader):
        # train discriminator D
        original_image, shadow_mask, shadow_free_image = y_
        
        original_image = original_image.to(device)
        shadow_mask = shadow_mask.to(device)
        shadow_free_image = shadow_free_image.to(device)
        
        ##############Discriminator #########
        
        real = torch.cat((original_image, shadow_mask), 1)
        prediction_real = G(real)
        loss_g = L1_loss(prediction_real,shadow_free_image)

        D.zero_grad()
        gt = torch.cat((original_image, shadow_free_image), 1)
        
        prediction_real_g, prediction_real_p = D(gt)
        
        error_real_g = bce(prediction_real_g,Variable(torch.ones(prediction_real_g.size())).to(device))
        error_real_p = bce(prediction_real_p,Variable(torch.ones(prediction_real_p.size())).to(device))
        
        loss_r = 0.995*loss_g + 0.0025 * error_real_g + 0.0025 * error_real_p
        
        ##########################################################################
        
        
        g_input = torch.cat((original_image, shadow_mask), 1)
        g_output = G(g_input)#fake_data

        g = torch.cat((original_image, g_output), 1)
        prediction_fake_g, prediction_fake_p = D(g)
        
        error_fake_g = bce(prediction_fake_g,Variable(torch.zeros(prediction_fake_g.size())).to(device))
        error_fake_p = bce(prediction_fake_p,Variable(torch.zeros(prediction_fake_p.size())).to(device))
        
        loss_f = 0.995*loss_g + 0.0025 * error_fake_g + 0.0025 * error_fake_p
        
        loss_j = (loss_r+loss_f)*0.5
        ############################################################## here
        loss_j.backward()
        D_optimizer.step()
        G_optimizer.step()
        joint_loss.append(loss_j.item())  #fake
        
        if((x_ % 20) ==0):
            global_step +=20
            print('[%d/%d] - loss_j: %.3f' % ((epoch + 1), train_epoch, torch.mean(torch.FloatTensor(joint_loss))))
            torchvision.utils.save_image(prediction_real, 'shadow_models/shadow_removals/'+str(epoch)+str(x_)+'test.png')
            torch.save(G.state_dict(), "shadow_models/shadow_removals/"+str(x_)+",generator_param.pkl")
            torch.save(D.state_dict(), "shadow_models/shadow_removals/"+str(x_)+",discriminator_param.pkl")
            vis.line(Y = [loss_j.item()], X = np.array([global_step]), win='train', update='append')
            time.sleep(0.5)
            
        if(epoch == train_epoch):
            torch.save(G.state_dict(), "shadow_models/shadow_removals/"+"epoch : "+str(train_epoch)+"f,generator_param.pkl")
            #torch.save(D.state_dict(), "09_25/histo"+"epoch : "+str(train_epoch)+"f,discriminator_param.pkl")

    epoch_end_time = time.time()
    per_epoch_ptime = epoch_end_time - epoch_start_time
    '''

    train_hist['joint_loss'].append(torch.mean(torch.FloatTensor(joint_loss)))
    train_hist['per_epoch_ptimes'].append(per_epoch_ptime)

end_time = time.time()
total_ptime = end_time - start_time
train_hist['total_ptime'].append(total_ptime)

print("Avg one epoch ptime: %.2f, total %d epochs ptime: %.2f" % (torch.mean(torch.FloatTensor(train_hist['per_epoch_ptimes'])), train_epoch, total_ptime))
print("Training finish!... save training results")
'''
