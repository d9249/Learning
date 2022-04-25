import torch
import torch.nn.functional as F

import numpy as np
import pdb, os, argparse
from scipy import misc
import imageio

from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import test_dataset
import os

parser = argparse.ArgumentParser()
parser.add_argument('--testsize', type=int, default=352, help='testing size')
parser.add_argument('--is_ResNet', type=bool, default=True, help='VGG or ResNet backbone')
opt = parser.parse_args()

dataset_path = '/data2/others/yun/data/SBU-shadow/SBU-Test/'

if opt.is_ResNet:
    model = CPD_ResNet()
    model.load_state_dict(torch.load('/data2/others/yun/shadow_detection/CPD/models/cpd-br_dice/_step:.256_epoch:.96_w.pth'))
'''
else:
    model = CPD_VGG()
    model.load_state_dict(torch.load('CPD.pth'))
'''
model.cuda()
#model.eval()

test_datasets = ['cpd_dice1']

for dataset in test_datasets:
    if opt.is_ResNet:
        save_path = '/data2/others/yun/shadow_detection/CPD/results/ResNet50/' + dataset + '/'
    else:
        save_path = './results/VGG16/' + dataset + '/'
    if not os.path.exists(save_path):
        os.makedirs(save_path)
    image_root = dataset_path + 'ShadowImages/'
    gt_root = dataset_path  + 'ShadowMasks/'
    test_loader = test_dataset(image_root, gt_root, opt.testsize)
    for i in range(test_loader.size):
        image, gt, name = test_loader.load_data()
        gt = np.asarray(gt, np.float32)
        gt /= (gt.max() + 1e-8)
        image = image.cuda()
        _, res = model(image)
        res = F.upsample(res, size=gt.shape, mode='bilinear', align_corners=False)
        res = res.sigmoid().data.cpu().numpy().squeeze()
        res = (res - res.min()) / (res.max() - res.min() + 1e-8)
        #misc.imsave(save_path+name, res)
        print(save_path+name)
        imageio.imwrite(save_path+name,res)
