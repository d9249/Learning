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
import torch.utils.data as data
from PIL import Image
import sys
import numpy as np
import pdb, os, argparse
os.environ["CUDA_VISIBLE_DEVICES"] = '0'
# G(z)

parser = argparse.ArgumentParser()
parser.add_argument('--test_image', type=str, default='100.png',help='testing image_name')
parser.add_argument('--original_path', type=str, default='../data/original/',help='training image size')
parser.add_argument('--mask_result_path', type=str, default='../result/fine_result/',help='training image size')
parser.add_argument('--shadow_less_path', type=str, default='../data/texture/',help='training image size')
parser.add_argument('--result_save_path', type=str, default='../result/remove_result/',help='training image size')

opt = parser.parse_args()


########################################CGAN shadow removal##############
batch_size = 1
dataset = shadow_triplets_loader()

data_loader = data.DataLoader(dataset, batch_size=batch_size)

def show_result(original,result,texture):
    fig = plt.figure(figsize=(15,5))
    rows = 1
    cols = 3   
    ax1 = fig.add_subplot(rows, cols, 1)
    ax1.imshow(original,aspect='auto')
    ax1.set_title('original')
    ax1.axis("off")
     
    ax2 = fig.add_subplot(rows, cols, 2)
    ax2.imshow(result,aspect='auto')
    ax2.set_title('Remove_result')
    ax2.axis("off")
    
    ax2 = fig.add_subplot(rows, cols, 3)
    ax2.imshow(texture,aspect='auto')
    ax2.set_title('texture')
    ax2.axis("off")
    
    plt.show()


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
G = Generator(4,3).cuda()
net = G
net.load_state_dict(torch.load(os.path.join('./model/380,generator_param.pkl'), map_location={'cuda:0': 'cpu'}))
# results save folder
if not os.path.isdir(opt.result_save_path):
    os.mkdir(opt.result_save_path)


trans = transforms.ToTensor()

# test image set
with torch.no_grad():
    original_image = Image.open(opt.original_path+opt.test_image)
    shadow_mask = Image.open(opt.mask_result_path+opt.test_image).convert('L')
    texture = Image.open(opt.shadow_less_path+opt.test_image)
    original_image = trans(original_image)
    shadow_mask = trans(shadow_mask)
    original_image = original_image.cuda()
    shadow_mask = shadow_mask.cuda()
    fake = torch.cat((original_image.unsqueeze(0), shadow_mask.unsqueeze(0)), 1)
    prediction_fake = net(fake)
    torchvision.utils.save_image(prediction_fake, opt.result_save_path+opt.test_image)
    ori = Image.open(opt.original_path+opt.test_image)
    result = Image.open(opt.result_save_path+opt.test_image)
    show_result(ori,result,texture)

print("Test finish!... ")
