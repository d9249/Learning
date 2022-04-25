import torch
import torch.nn.functional as F
from torch.autograd import Variable
import torch.nn as nn

import numpy as np
import pdb, os, argparse
from datetime import datetime

from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import get_loader
from utils import clip_gradient, adjust_lr
import torchvision

import visdom

parser = argparse.ArgumentParser()
parser.add_argument('--epoch', type=int, default=500, help='epoch number')
parser.add_argument('--lr', type=float, default=1e-4, help='learning rate')
parser.add_argument('--batchsize', type=int, default=30, help='training batch size')
parser.add_argument('--trainsize', type=int, default=352, help='training dataset size')
parser.add_argument('--clip', type=float, default=0.5, help='gradient clipping margin')
parser.add_argument('--is_ResNet', type=bool, default=True, help='VGG or ResNet backbone')
parser.add_argument('--decay_rate', type=float, default=0.1, help='decay rate of learning rate')
parser.add_argument('--decay_epoch', type=int, default=50, help='every n epochs decay learning rate')
parser.add_argument('--best_loss', type=float, default=1.000, help='every n epoch best loss')
parser.add_argument('--best_epoch', type=int, default=1, help='every n best_epoch')
opt = parser.parse_args()

image_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/original/'
height_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/height/'
gt_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/texture/'

train_loader = get_loader(image_root, height_root,gt_root, batchsize=opt.batchsize, trainsize=opt.trainsize)
total_step = len(train_loader)

for i, pack in enumerate(train_loader):
    #Eif i == 1 or i == 2:
        print(i)