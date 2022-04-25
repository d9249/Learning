import os, time
import matplotlib.pyplot as plt
import itertools
import pickle
import imageio
from torch.autograd import Variable
import torch
from test_image_loader import *
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from torchvision import datasets, transforms
import torchvision
from torch.autograd import Variable
import torch.utils.data as Data
from PIL import Image
import sys
import numpy

device = torch.device("cuda:1" if torch.cuda.is_available() else "cpu")
# G(z)

########################################CGAN shadow removal##############
batch_size = 1
dataset = shadow_triplets_loader()

data_loader = Data.DataLoader(dataset, batch_size=batch_size)

ckpt_path = './'
exp_name = 'models/shadow_removal/'
args = {
    'snapshot' : '80'
}

class Flatten(nn.Module):
    def forward(self, x):
        x = x.view(x.size()[0], -1)
        return x
    
def weights_init_normal(m):
    classname = m.__class__.__name__
    if classname.find('Conv') != -1:
        torch.nn.init.normal_(m.weight.data, 0.0, 0.02)
    elif classname.find('BatchNorm') != -1:
        torch.nn.init.normal_(m.weight.data, 1.0, 0.02)
        torch.nn.init.constant_(m.bias.data, 0.0)

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

        # Initial convolution block       
        model = [   nn.ReflectionPad2d(3),
                    nn.Conv2d(input_nc, 64, 7),
                    nn.InstanceNorm2d(64),
                    nn.ReLU(inplace=True) ]
        
        # Downsampling
        in_features = 64
        out_features = in_features*2
        for _ in range(2):
            model += [  nn.Conv2d(in_features, out_features, 3, stride=2, padding=1),
                        nn.InstanceNorm2d(out_features),
                        nn.ReLU(inplace=True) ]
            in_features = out_features
            out_features = in_features*2

        # Residual blocks
        i=0
        for i in range(n_residual_blocks):
            
            model += [ResidualBlock(in_features)]
            
        # Upsampling
        out_features = in_features//2
        for _ in range(2):
            model += [  nn.ConvTranspose2d(in_features, out_features, 3, stride=2, padding=1, output_padding=1),
                        nn.InstanceNorm2d(out_features),
                        nn.ReLU(inplace=True) ]
            
            in_features = out_features
            out_features = in_features//2
        
        # Output layer
        model += [  nn.ReflectionPad2d(3),
                    nn.Conv2d(64, output_nc, 7),
                    nn.Tanh() ]
        self.model = nn.Sequential(*model)
        
    def forward(self, x):
        return self.model(x)
	
	# network
G = Generator(4,3).to(device)
net = G

net.load_state_dict(torch.load(os.path.join(ckpt_path, exp_name, args['snapshot']+',generator_param' + '.pkl'), map_location=device))

def rmse(y, y_hat,mask,k):
    """Compute root mean squared error, shadow region and non-shadow region"""
    #y = generated image, y_hat = Ground Truth image
    ####shadow_region error########
    
    shadow_region = (y*mask)
    shadow_region_gt = (y_hat*mask)
    #torchvision.utils.save_image(shadow_region, 'shadow/'+str(k)+'test.png')
    ####non-shadow_region error########
    non_shadow_region = y*(torch.ones(y.size()).to(device)-mask)
    #torchvision.utils.save_image(non_shadow_region, 'non-shadow/'+str(k)+'test.png')
    non_shadow_region_gt = y_hat*(torch.ones(y_hat.size()).to(device)-mask)
    error_shadow = torch.sqrt(torch.mean((shadow_region - shadow_region_gt).pow(2)))
    error_non_shadow = torch.sqrt(torch.mean((non_shadow_region - non_shadow_region_gt).pow(2)))
    return error_shadow,error_non_shadow

# results save folder
if not os.path.isdir('result'):
    os.mkdir('result')

print('test start!')

start_time = time.time()
k=0
d=0
print('test start!')
shadow_regions=0
non_shadows=0
        
ori_shadows=0
ori_nons=0

i=0
# test image set
with torch.no_grad():
    for x_, y_ in enumerate(data_loader):
        # train discriminator D
        original_image, shadow_mask,name = y_
        original_image = original_image.to(device)
        shadow_mask = shadow_mask.to(device)
        fake = torch.cat((original_image, shadow_mask), 1)
        prediction_fake = net(fake)
        torchvision.utils.save_image(prediction_fake, './result/'+os.path.basename(str(name)[:-5])+'_test_.png')
        
        #prediction_fake = colors.rgb_to_lab(prediction_fake)
        
        #shadow_free_image = colors.rgb_to_lab(shadow_free_image)
        #original_image = colors.rgb_to_lab(original_image)
        #shadow_region,non_shadow = rmse(prediction_fake,shadow_free_image,shadow_mask,x_)
        #ori_shadow,ori_non = rmse(original_image,shadow_free_image,shadow_mask,x_)
        
        #shadow_regions+=shadow_region
        #non_shadows+=non_shadow
        
        #ori_shadows+=ori_shadow
        #ori_nons+=ori_non
        
        
    #epoch_end_time = time.time()
    #per_epoch_ptime = epoch_end_time - epoch_start_time
    
    #fixed_p = 'shadow_result/Fixed_results/shadow_cGAN_' + str(epoch + 1) + '.png'
    #show_result((epoch+1), save=True, path=fixed_p)
'''
print("----------------------------------------------------------")
print("Test_result(shadow) : "+str(shadow_regions.item()/400.0))
print("original difference(shadow) : "+str(ori_shadows.item()/400.0))
print("Test_result(non) : "+str(non_shadows.item()/400.0))
print("original difference(non) : "+str(ori_nons.item()/400.0))
print("----------------------------------------------------------")
print("Test_result : "+str((shadow_regions.item()+non_shadows.item())/400.0))
print("original difference : "+str((ori_shadows.item()+ori_nons.item())/400.0))
'''

print("Test finish!... ")