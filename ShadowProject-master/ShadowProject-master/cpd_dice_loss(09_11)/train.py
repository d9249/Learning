import torch
import torch.nn.functional as F
from torch.autograd import Variable

import numpy as np
import pdb, os, argparse
from datetime import datetime

from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import get_loader
from utils import clip_gradient, adjust_lr
import torchvision


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
parser.add_argument('--best_loss', type=float, default=1.0, help='best_loss')
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

image_root = '/data2/others/yun/data/SBU-shadow/SBUTrain4KRecoveredSmall/ShadowImages/'
gt_root = '/data2/others/yun/data/SBU-shadow/SBUTrain4KRecoveredSmall/ShadowMasks/'
train_loader = get_loader(image_root, gt_root, batchsize=opt.batchsize, trainsize=opt.trainsize)
total_step = len(train_loader)

CE = torch.nn.BCEWithLogitsLoss()

if opt.is_ResNet:
  save_path = 'models/cpd-br-dice-deconv/'
else:
  save_path = 'models/CPD_VGG/'

if not os.path.exists(save_path):
        os.makedirs(save_path)


def SoftDiceLoss(logits,targets):
    smooth = 1.
    logits = F.sigmoid(logits)
    iflat = logits.view(-1)
    tflat = targets.view(-1)
    intersection = (iflat * tflat).sum()
    return 1 - ((2. * intersection + smooth) /(iflat.sum() + tflat.sum() + smooth))
        
#model.load_state_dict(torch.load(os.path.join('/data2/others/yun/shadow_detection/CPD/models/CPD_Resnet/_step:.171_epoch:.486_w.pth')))
def train(train_loader, model, optimizer, epoch):
    model.train()
    for i, pack in enumerate(train_loader, start=1):
        optimizer.zero_grad()
        images, gts = pack
        images = Variable(images)
        gts = Variable(gts)
        images = images.cuda()
        gts = gts.cuda()

        atts, dets = model(images)
        
        loss1 = 0.5*CE(atts, gts) +0.5*SoftDiceLoss(atts, gts)
        loss2 = 0.5*CE(dets, gts) + 0.5*SoftDiceLoss(dets, gts)
        loss = loss1 + loss2
        loss.backward()

        clip_gradient(optimizer, opt.clip)
        optimizer.step()

        if i%10 == 0 :
            print('{} Epoch [{:03d}/{:03d}], Step [{:04d}/{:04d}], Loss1: {:.4f} Loss2: {:0.4f}'.
                  format(datetime.now(), epoch, opt.epoch, i, total_step, loss1.data, loss2.data))
            #torchvision.utils.save_image(atts,save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_atts_test.png')
        
        if (opt.best_loss>loss2.data):
            opt.best_loss = loss2.data
            opt.best_epoch = epoch
            #torchvision.utils.save_image(atts, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_atts_test.png')
            torchvision.utils.save_image(dets, save_path+'_epoch:_'+str(epoch)+'_iter_'+str(i)+'_dets_test.png')
            torch.save(model.state_dict(), save_path +'_step:'+'.%d' %i +'_epoch:'+'.%d' % epoch + '_w.pth')

    torch.save(model.state_dict(), save_path +'_step:'+'.%d' %i +'_epoch:'+'.%d' % epoch + '_w.pth')
    
print("Let's go!")
for epoch in range(1, opt.epoch):
    adjust_lr(optimizer, opt.lr, epoch, opt.decay_rate, opt.decay_epoch)
    train(train_loader, model, optimizer, epoch)
