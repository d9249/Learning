import torch
import torch.nn.functional as F

import numpy as np
import pdb, os, argparse
from scipy import misc
import os
from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import test_dataset
from guided_filter_pytorch.guided_filter import GuidedFilter
from PIL import Image
import torchvision


os.environ["CUDA_VISIBLE_DEVICES"] = '1'

parser = argparse.ArgumentParser()
parser.add_argument('--testsize', type=int, default=352, help='testing size')
parser.add_argument('--is_ResNet', type=bool, default=True, help='VGG or ResNet backbone')
opt = parser.parse_args()

if opt.is_ResNet:
    model = CPD_ResNet()
    model.load_state_dict(torch.load('checkpoint.pth'))
else:
    model = CPD_VGG()
    model.load_state_dict(torch.load('CPD.pth'))

model.cuda()
model.eval()

test_datasets = ['gf']

for dataset in test_datasets:
    if opt.is_ResNet:
        save_path = './results/ResNet50/' + dataset + '/'
    else:
        save_path = './results/VGG16/' + dataset + '/'
    if not os.path.exists(save_path):
        os.makedirs(save_path)
    image_root = './data/test0.5/'
    gt_root = './data/original/'
    height_root = './data/height_map/'
    test_loader = test_dataset(image_root, gt_root,height_root, opt.testsize)
    for i in range(test_loader.size):
        image, gt, height,name = test_loader.load_data()
        #gt = np.asarray(gt, np.float32)
        #gt /= (gt.max() + 1e-8)
        #images = torch.cat((image,height),1).cuda()
        #_, res = model(images)
        #res = F.upsample(res, size=gt.shape, mode='bilinear', align_corners=False)
        #res = res.sigmoid().data.cpu().numpy().squeeze()
        #res = (res - res.min()) / (res.max() - res.min() + 1e-8)
        #res = res>0.5
        result = GuidedFilter(100, 0.005)(gt, image)
        print(result.size())
        torchvision.utils.save_image(result,'/home/sonic/hj3/CPD-channel-height/guided_result/'+str(name[:-4])+'.png')
        #result = result.sigmoid().data.cpu().numpy().squeeze()
        #print(result.shape)
        #misc.imsave(save_path+name, result)     