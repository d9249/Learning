import torch
import torch.nn.functional as F

import numpy as np
import pdb, os, argparse
from scipy import misc

from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import test_dataset
import torchvision

parser = argparse.ArgumentParser()
parser.add_argument('--testsize', type=int, default=1600, help='testing size')
parser.add_argument('--is_ResNet', type=bool, default=True, help='VGG or ResNet backbone')
opt = parser.parse_args()

dataset_path = '/home/sonic/hj3/data/height_second/test'
dataset_path = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/test'

trainedmodel_path = '/home/juwan/SharedFolder/MyGitHub/ShadowProject/CPD09_25_BY_WANI/models/height_second_channel/_step:.134_epoch:.416_w.pth'
if opt.is_ResNet:
    model = CPD_ResNet()
    model.load_state_dict(torch.load                          ('/home/juwan/SharedFolder/MyGitHub/ShadowProject/CPD09_25_BY_WANI/models/height_second_channel/_step:.134_epoch:.499_w.pth'))
#    model.load_state_dict(torch.load('/home/sonic/hj3/CPD_height/models/height_second_channel/_step:.134_epoch:.416_w.pth'))
        
else:
    model = CPD_VGG()
    model.load_state_dict(torch.load('CPD.pth'))

model.cuda()
model.eval()

test_datasets = ['test']

for dataset in test_datasets:
    if opt.is_ResNet:
#        save_path = '/home/sonic/hj3/CPD_height/result/test/'
        save_path = '/home/juwan/SharedFolder/MyGitHub/ShadowProject/CPD09_25_BY_WANI/result/test/'
    else:
        save_path = './results/VGG16/' + dataset + '/'
    if not os.path.exists(save_path):
        os.makedirs(save_path)
    image_root = '/home/sonic/hj3/data/test_height/height/original/'
    mask_root = '/home/sonic/hj3/data/test_height/test_mask/s/'
    height_root = '/home/sonic/hj3/data/test_height/'
    
    image_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/test/original/'
    mask_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/test/mask/'
    height_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/test/height_map/'
 
    
    test_loader = test_dataset(image_root, mask_root,height_root, opt.testsize)
    
    for i in range(test_loader.size):
        if i == 1:
            image, mask,height, name = test_loader.load_data()
            mask = np.asarray(mask, np.float32)
            mask /= (mask.max() + 1e-8)
            image = torch.cat([image,height],1)
            image = image.cuda()
            _, res = model(image)
            res = F.upsample(res, size=mask.shape, mode='bilinear', align_corners=False)
            res = res.sigmoid().data.cpu().numpy().squeeze()
            res = (res - res.min()) / (res.max() - res.min() + 1e-8)
            res = res>0.5
            #torchvision.utils.save_image(res,'result(no_height)/'+name+'.png')
            misc.imsave(save_path+name, res*255.0)
            print("save iamge", save_path+name)
