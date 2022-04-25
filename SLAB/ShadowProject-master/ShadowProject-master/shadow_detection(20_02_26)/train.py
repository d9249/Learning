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
from pytorchtools import EarlyStopping

os.environ["CUDA_VISIBLE_DEVICES"]="1"

parser = argparse.ArgumentParser()
parser.add_argument('--epoch', type=int, default=500, help='epoch number')
parser.add_argument('--lr', type=float, default=1e-4, help='learning rate')
parser.add_argument('--batchsize', type=int, default=16, help='training batch size')
parser.add_argument('--trainsize', type=int, default=352, help='training dataset size')
parser.add_argument('--clip', type=float, default=0.5, help='gradient clipping margin')
parser.add_argument('--is_ResNet', type=bool, default=True, help='VGG or ResNet backbone')
parser.add_argument('--decay_rate', type=float, default=0.1, help='decay rate of learning rate')
parser.add_argument('--decay_epoch', type=int, default=50, help='every n epochs decay learning rate')
parser.add_argument('--best_loss', type=float, default=1.000, help='every n epoch best loss')
parser.add_argument('--best_epoch', type=int, default=1, help='every n best_epoch')
opt = parser.parse_args()

print('Learning Rate: {} ResNet: {}'.format(opt.lr, opt.is_ResNet))
# build models
if opt.is_ResNet:
    model = CPD_ResNet()
else:
    model = CPD_VGG()

model.cuda()
params = model.parameters()
optimizer = torch.optim.Adam(params, opt.lr)

image_root = '/data2/others/yun/data/Data_Height/train/original/'
gt_root = '/data2/others/yun/data/Data_Height/train/mask/'
height_root = '/data2/others/yun/data/Data_Height/train/height/'

train_loader, valid_loader = get_loader(image_root, height_root,gt_root, batchsize=opt.batchsize, trainsize=opt.trainsize)
total_step = len(train_loader)

CE = torch.nn.BCEWithLogitsLoss()

if opt.is_ResNet:
    save_path = 'models/channel/'
else:
    save_path = 'models/CPD_VGG/'


if not os.path.exists(save_path):
        os.makedirs(save_path)

'''
ckpt_path = '/home/heejin/shadow_detection/CPD/'
exp_name = 'models/'
args = {
    'step' : '30'
}


model.load_state_dict(torch.load(os.path.join(ckpt_path, exp_name,'_step:.'+args['step']+'_w.pth')))
'''

def train(train_loader, model, optimizer, epoch, patience):
      train_losses = []
      # to track the validation loss as the model trains
      valid_losses = []
      # to track the average training loss per epoch as the model trains
      avg_train_losses = []
      # to track the average validation loss per epoch as the model trains
      avg_valid_losses = [] 
      early_stopping = EarlyStopping(patience=patience, verbose=True)
      
      for epoch in range(1, opt.epoch):
        adjust_lr(optimizer, opt.lr, epoch, opt.decay_rate, opt.decay_epoch)
        
        
      # initialize the early_stopping object
        
        model.train()
        for i, pack in enumerate(train_loader, start=1):
            optimizer.zero_grad()
            images, heights,gts = pack
            images = Variable(images)
            heights = Variable(heights)
            gts = Variable(gts)
            images = images.cuda()
            heights = heights.cuda()
            gts = gts.cuda()
            
            cat = torch.cat([images,heights],1)
            atts, dets=model(cat)
            
            loss1 = CE(atts, gts)
            loss2 = CE(dets, gts)
            loss = loss1 + loss2
            loss.backward()
            train_losses.append(loss.item())
            clip_gradient(optimizer, opt.clip)
            optimizer.step()
            
            if i % 10 == 0 or i == total_step:
                print('{} Epoch [{:03d}/{:03d}], Step [{:04d}/{:04d}], Loss1: {:.4f} Loss2: {:0.4f}, Loss:{:.4f}'.
                      format(datetime.now(), epoch, opt.epoch, i, total_step, loss1.data, loss2.data, loss.data))
        
        model.eval()
        for images,heights,gts in valid_loader:
            images = Variable(images)
            heights = Variable(heights)
            gts = Variable(gts)
            images = images.cuda()
            heights = heights.cuda()
            gts = gts.cuda()
            
            cat = torch.cat([images,heights],1)
            atts, dets=model(cat)
            
            loss1 = CE(atts, gts)
            loss2 = CE(dets, gts)
            loss = loss1 + loss2
            valid_losses.append(loss2.item())
        
        train_loss = np.average(train_losses)
        valid_loss = np.average(valid_losses)
        avg_train_losses.append(train_loss)
        avg_valid_losses.append(valid_loss)
        
        train_losses = []
        valid_losses = []
        print('{} Epoch [{:03d}/{:03d}], train_Loss:{:.4f},valid_Loss:{:.4f} '.format(datetime.now(), epoch, opt.epoch, train_loss, valid_loss))
        early_stopping(valid_loss, model)
  
      model.load_state_dict(torch.load('checkpoint.pth'))
      return model, avg_train_losses, avg_valid_losses
  
print("Let's go!")
patience = 20
model, train_loss, valid_loss = train(train_loader, model, optimizer, opt.epoch, patience)
    