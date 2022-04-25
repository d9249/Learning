import torchvision
import torch.nn.functional as F
from model.FusionNet import * 
from model.SimpleUNet import *
import numpy as np
import matplotlib.pyplot as plt
from datetime import datetime
from fine_data import get_loader
from fine_data import test_dataset
import os
import argparse
import torchvision.transforms.functional as F
import torchvision.transforms as transforms
from skimage import io
from PIL import Image

device = torch.device('cuda:0')

parser = argparse.ArgumentParser()
parser.add_argument('--test_size', type=int, default=400, help='training image size')
parser.add_argument('--original_path', type=str, default='../data/original/',help='training image size')
parser.add_argument('--detected_path', type=str, default='../result/rough_result/',help='training image size')
parser.add_argument('--mask_path', type=str, default='../data/mask/',help='training image size')
parser.add_argument('--test_image', type=str, default='100.png',help='training image size')
parser.add_argument('--result_save_path', type=str, default='../result/fine_result/')
opt = parser.parse_args()

def show_result(original,mask,rough_result,fine_result):
    fig = plt.figure(figsize=(15,20))
    ax1 = fig.add_subplot(2,2,1)
    ax1.imshow(original,aspect='auto')
    ax1.set_title('original')
    ax1.axis("off")
    
    ax2 = fig.add_subplot(2,2,2)
    ax2.imshow(mask,cmap = plt.get_cmap('gray'),aspect='auto')
    ax2.set_title('mask')
    ax2.axis("off")
    
    ax3 = fig.add_subplot(2,2,3)
    ax3.imshow(rough_result,cmap = plt.get_cmap('gray'),aspect='auto')
    ax3.set_title('rough_result')
    ax3.axis("off")

    ax4 = fig.add_subplot(2,2,4)
    ax4.imshow(fine_result,aspect='auto')
    ax4.set_title('fine_result')
    ax4.axis("off")
     
    plt.show()

trans = transforms.ToTensor()

#-----------------------------------------------------
model_path = './models/fine/_step_161 epoch_328 loss_ 0.3133.pth'

if not os.path.exists(opt.result_save_path):
    os.makedirs(opt.result_save_path)

model = UnetGeneratorOriginal(2, 2, 64)
model.cuda(device)



model.load_state_dict(torch.load(model_path,map_location=lambda storage, loc: storage))


img_transform = transforms.Compose([transforms.Resize((400,400)),transforms.Grayscale(num_output_channels=1),transforms.ToTensor()])

original_image = Image.open(opt.original_path+opt.test_image)
shadow_mask = Image.open(opt.mask_path+opt.test_image)
detected = Image.open(opt.detected_path+opt.test_image)


original_image = img_transform(original_image)
detected = img_transform(detected)
    
original_image = original_image.cuda()
detected = detected.cuda()

images = torch.cat((original_image.unsqueeze(0),detected.unsqueeze(0)),1)

res = model(images)
_, res = torch.chunk(res, 2, dim=1)
res = res.sigmoid().data.cpu().numpy().squeeze()
res = (res - res.min()) / (res.max() - res.min() + 1e-8)
res = res>0.5
io.imsave(opt.result_save_path+opt.test_image, res+0.0)

ori = Image.open(opt.original_path+opt.test_image)
result = Image.open(opt.result_save_path+opt.test_image)
rough_result = Image.open(opt.detected_path+opt.test_image)
show_result(ori,shadow_mask,rough_result,result)
