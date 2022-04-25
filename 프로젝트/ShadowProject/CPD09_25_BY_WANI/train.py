import torch
import torch.nn.functional as F
from torch.autograd import Variable
import torch.nn as nn
import sys

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
parser.add_argument('--inputchannel', type=int, default=2, help='4: RGB+Height, 2: Gray+Height')
parser.add_argument('--cudaindex', type=int, default=1, help='GPU No')
opt = parser.parse_args()

#----------------
# opt.trainsize = 400
if (opt.inputchannel != 2 and opt.inputchannel != 4):
    print("Input Channel Error")
    sys.exit(0)
#------------------

cuda_no = "cuda:%d"%opt.cudaindex
device = torch.device(cuda_no if torch.cuda.is_available() else "cpu")

print (opt)

###############################################################################
vis = visdom.Visdom()
vis.close(env="main")  #CPD_9_25_BY_WANI")

def loss_tracker(loss_plot, loss_value, num):
    '''num, loss_value, are Tensor'''
    vis.line(X=num,
             Y=loss_value,
             win = loss_plot,
             update='append'
             )

# 2line Loss Plot 
sub_title = "[Cuda:%d][InputChannel:%d]"% (opt.cudaindex, opt.inputchannel)
loss_plt = vis.line(Y=torch.Tensor([[1., 1.,1.]]), X=torch.Tensor([0]), opts=dict(title='Loss Tracker'+sub_title, legend=['loss', 'loss1', 'loss2'], showlegend=True))
acc_plt = vis.line(Y=torch.Tensor(1).zero_(),opts=dict(title='Accuracy'+sub_title, legend=['Acc'], showlegend=True))
#################################################################################

print('Learning Rate: {} ResNet: {}'.format(opt.lr, opt.is_ResNet))
# build models
if opt.is_ResNet:
    model = CPD_ResNet(inchannel = opt.inputchannel)
else:
    model = CPD_VGG()

model.cuda(device)
params = model.parameters()
optimizer = torch.optim.Adam(params, opt.lr)

image_root = '/data2/others/yun/data/Data_Height/train/original/'
height_root = '/data2/others/yun/data/Data_Height/train/height/'
gt_root = '/data2/others/yun/data/Data_Height/train/mask/'


image_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/original/'
height_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/height/'
gt_root = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/train/mask/'

train_loader = get_loader(image_root, height_root,gt_root, batchsize=opt.batchsize, 
                            trainsize=opt.trainsize, inputchannel=opt.inputchannel)
total_step = len(train_loader)

CE = torch.nn.BCEWithLogitsLoss()

#-----------------------------------------------------
# make folder name 
a = datetime.now()
folder_tag = "{:02d}.{:02d}-{:02d}.{:02d}".format(a.month, a.day,a.hour,a.minute)
if opt.inputchannel == 4:
    folder_tag += "_RGB"
else:
    folder_tag += "_GRAY"
#

if opt.is_ResNet:
    save_path = 'ResnetLearning/' + folder_tag  + '/'
    save_path_pth = save_path + "pth"
else:
    save_path = 'CPD_VGG_Learning/' + folder_tag + '/'
#-----------------------------------------------------

if not os.path.exists(save_path):
        os.makedirs(save_path)
        os.makedirs(save_path_pth)


def train(train_loader, model, optimizer, epoch):
    
    model.train()
    cur_loss = 0.0
    cur_loss1 = 0.0
    cur_loss2 = 0.0
    for i, pack in enumerate(train_loader, start=1):
        
        optimizer.zero_grad()
        images, heights,gts = pack
        #images : [30, 1, 352, 352]
        #heights : [30, 1, 352, 352]
        #gts : [30, 1, 352, 352]
        images = Variable(images)

        heights = Variable(heights)
        gts = Variable(gts)
        images = images.cuda(device)
        heights = heights.cuda(device)
        gts = gts.cuda(device)
        
        cat = torch.cat([images,heights],1)
        # cat : [30, 2, 352, 352]
        atts, dets=model(cat)#,x2_1,x3_1,x4_1,x2_2,x3_2,x4_2 = model(images)
        #criterion = SoftDiceLoss().to(device)
        loss1 = CE(atts, gts)
        loss2 = CE(dets, gts)
        loss = loss1 + loss2
        loss.backward()
        clip_gradient(optimizer, opt.clip)
        optimizer.step()
        
        # sum loss by juwan
        cur_loss += loss.item()
        cur_loss1 += loss1.item()
        cur_loss2 += loss2.item()
        
        if i % 30 == 0: #or i == total_step:
            print('{} Epoch [{:03d}/{:03d}], Step [{:04d}/{:04d}], Loss1: {:.4f} Loss2: {:0.4f}, Loss:{:.4f}'.
                  format(datetime.now(), epoch, opt.epoch, i, total_step, loss1.data, loss2.data, loss.data))
            loss_tracker(loss_plt, torch.Tensor([loss1.data, loss2.data]),torch.Tensor([epoch, epoch]))

            
        if(opt.best_loss>loss2.data):
            opt.best_loss = loss2.data
            opt.best_epoch = epoch
            torch.save(model.state_dict(), save_path +'pth/_step:'+'.%03d batch' %i +'_epoch:'+'.%03d' % epoch + ' loss2: %.4f'%opt.best_loss+'_w.pth')
            torchvision.utils.save_image(atts, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_attention_test.png')
            torchvision.utils.save_image(dets, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_detetcion_test.png')
    
#    print('{} Epoch [{:03d}/{:03d}], Loss1: {:.4f} Loss2: {:0.4f}, Average Loss:{:.4f}'.
#                  format(datetime.now(), epoch, opt.epoch, loss1.data, loss2.data, cur_loss/len(train_loader))) 
    print('{} Epoch [{:03d}/{:03d}], Loss1: {:.4f} Loss2: {:0.4f}, Average Loss:{:.4f}'.
                  format(datetime.now(), epoch, opt.epoch, cur_loss1/len(train_loader), cur_loss2/len(train_loader), cur_loss/len(train_loader))) 
    
    loss_tracker(loss_plt, torch.Tensor( [[cur_loss/len(train_loader), cur_loss1/len(train_loader), cur_loss2/len(train_loader)]]), torch.Tensor([[epoch, epoch, epoch]]))            
    torch.save(model.state_dict(), save_path +'_step:'+'.%d' %i +'_epoch:'+'.%d' % epoch + '_w.pth')
    torchvision.utils.save_image(atts, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_attention_test.png')
    torchvision.utils.save_image(dets, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_detetcion_test.png')

          
print("Let's go!")
for epoch in range(1, opt.epoch):
    adjust_lr(optimizer, opt.lr, epoch, opt.decay_rate, opt.decay_epoch)
    train(train_loader, model, optimizer, epoch)
    print(opt.best_loss)
    
