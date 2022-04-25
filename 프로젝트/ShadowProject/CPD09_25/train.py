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

device = torch.device("cuda:1" if torch.cuda.is_available() else "cpu")

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

print('Learning Rate: {} ResNet: {}'.format(opt.lr, opt.is_ResNet))
# build models
if opt.is_ResNet:
    model = CPD_ResNet()
else:
    model = CPD_VGG()

model.cuda(device)
params = model.parameters()
optimizer = torch.optim.Adam(params, opt.lr)

image_root = '/data2/others/yun/data/Data_Height/train/original/'
height_root = '/data2/others/yun/data/Data_Height/train/height/'
gt_root = '/data2/others/yun/data/Data_Height/train/texture/'

train_loader = get_loader(image_root, height_root,gt_root, batchsize=opt.batchsize, trainsize=opt.trainsize)
total_step = len(train_loader)

CE = torch.nn.BCEWithLogitsLoss()

if opt.is_ResNet:
    save_path = 'models/height_second_channel/'
else:
    save_path = 'models/CPD_VGG/'


if not os.path.exists(save_path):
        os.makedirs(save_path)

def train(train_loader, model, optimizer, epoch):
    
    model.train()
    for i, pack in enumerate(train_loader, start=1):
        
        optimizer.zero_grad()
        images, heights,gts = pack
        images = Variable(images)
        heights = Variable(heights)
        gts = Variable(gts)
        images = images.cuda(device)
        heights = heights.cuda(device)
        gts = gts.cuda(device)
        
        cat = torch.cat([images,heights],1)
        atts, dets=model(cat)#,x2_1,x3_1,x4_1,x2_2,x3_2,x4_2 = model(images)
        #criterion = SoftDiceLoss().to(device)
        
        loss1 = CE(atts, gts)
        loss2 = CE(dets, gts)
        loss = loss1 + loss2
        loss.backward()
        clip_gradient(optimizer, opt.clip)
        optimizer.step()
        
        if i % 10 == 0 or i == total_step:
            print('{} Epoch [{:03d}/{:03d}], Step [{:04d}/{:04d}], Loss1: {:.4f} Loss2: {:0.4f}, Loss:{:.4f}'.
                  format(datetime.now(), epoch, opt.epoch, i, total_step, loss1.data, loss2.data, loss.data))
            
            
        if(opt.best_loss>loss2.data):
            opt.best_loss = loss2.data
            opt.best_epoch = epoch
            torch.save(model.state_dict(), save_path +'_step:'+'.%d' %i +'_epoch:'+'.%d' % epoch + '_w.pth')
            torchvision.utils.save_image(atts, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_attention_test.png')
            torchvision.utils.save_image(dets, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_detetcion_test.png')
            
    torch.save(model.state_dict(), save_path +'_step:'+'.%d' %i +'_epoch:'+'.%d' % epoch + '_w.pth')
    torchvision.utils.save_image(atts, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_attention_test.png')
    torchvision.utils.save_image(dets, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_detetcion_test.png')
            
print("Let's go!")
for epoch in range(1, opt.epoch):
      adjust_lr(optimizer, opt.lr, epoch, opt.decay_rate, opt.decay_epoch)
      train(train_loader, model, optimizer, epoch)
    #print(opt.best_loss)
    