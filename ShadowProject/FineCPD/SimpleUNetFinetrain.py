import torchvision
from model.FusionNet import * 
from model.SimpleUNet import *
import numpy as np
import matplotlib.pyplot as plt
from datetime import datetime
from fine_data import get_loader
import os
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("--network",type=str,default="fusionnet",help="choose between fusionnet & unet")
parser.add_argument('--epoch', type=int, default=100, help='epoch number')
parser.add_argument("--batch_size",type=int,default=12,help="batch size")
parser.add_argument('--train_size', type=int, default=400, help='training image size')
parser.add_argument("--num_gpu",type=int,default=1,help="number of gpus")           # DON'T USE parallel
parser.add_argument('--foldername', type=str, default="", help='foldername')
parser.add_argument('--cudaid', type=int, default=3, help='GPU id')
args = parser.parse_args()

__BIN_LOSS__ = False
# hyperparameters
batch_size = args.batch_size
img_size = args.train_size
lr = 0.0001
best_loss = 1.00
best_epoch = 1

#################################################################################
#  Folder 
# input : image_root (gray) , detected_root (detected shadow image : binary)
# output : image_root (gray), gt_root (Ground Truth : binary)
#-----------------------------------------
ROOT_IMAGE_FOLDER = '/home/juwan/SharedFolder/OpenDataSet/(20190831)height_map2/'
image_root = ROOT_IMAGE_FOLDER + 'train/original/'  # Original Image with Shadow (RGB)
gt_root = ROOT_IMAGE_FOLDER + 'train/mask/'         # GT Mask Image (Binary)
height_root = ROOT_IMAGE_FOLDER + 'train/height/'   # 필요 없을 것 같음....           # Height Image  
# CPD로 1차 검출한 그림자 데이타
detected_root = '/home/juwan/SharedFolder/MyGitHub/ShadowProject/CPD09_25_BY_WANI/result/train/'
'''
make output folder  
'''
a = datetime.now()
timestamp = "{:02d}.{:02d}-{:02d}.{:02d}".format(a.month, a.day,a.hour,a.minute)
save_path = 'Learning/' + args.foldername + timestamp + '/'
save_path_pth = save_path + "best_pth"
#-----------------------------------------------------
if not os.path.exists(save_path):
    os.makedirs(save_path)
    os.makedirs(save_path_pth)

cuda_no = "cuda:%d"%args.cudaid
device = torch.device(cuda_no if torch.cuda.is_available() else "cpu")

'''
Prepare Model

if args.network == "fusionnet":
	generator = nn.DataParallel(FusionGenerator(3,3,64),device_ids=[i for i in range(args.num_gpu)]).cuda()
elif args.network == "unet":
	generator = nn.DataParallel(UnetGenerator(3,3,64),device_ids=[i for i in range(args.num_gpu)]).cuda()
'''
# initialize Generator
model = UnetGenerator(2, 2, 64)

print("Device: ", device)

model.cuda(device)

# loss function & optimizer
params = model.parameters()
model_optimizer = torch.optim.Adam(params,lr = lr)
mse_loss_func = nn.MSELoss()

#loss_func = nn.CrossEntropyLoss()
#optimizer = torch.optim.SGD(params, lr = lr, monentum = 0.99)  
binary_loss_func = nn.BCEWithLogitsLoss()


#CE = torch.nn.BCEWithLogitsLoss()

# load pretrained model

try:
    generator = torch.load('./model/{}.pkl'.format(args.network))
    print("\n--------model restored--------\n")
except:
    print("\n--------model not restored--------\n")
    pass

'''
Prepare Training Data
'''
train_loader = get_loader(image_root, detected_root, gt_root, batchsize=batch_size, 
                            trainsize=img_size, inputchannel=2)
total_step = len(train_loader)


print("Let's go!")

# Save Learning Info.
f = open(save_path + "readme.txt", "w")
f.write("\r\n"),f.write(str(args)),f.write("\r\n")
f.write("image_root: " + image_root), f.write("\r\ngt_root: " + gt_root)
f.write("\r\nheight_root: " + height_root), f.write("\r\ndetected_root:" + detected_root), f.write("\r\n")
f.close()
print("Save Learning Information")

# Training
for epoch in range(1, args.epoch):
    model.train()
    total_loss = 0.0
    for i, pack in enumerate(train_loader, start = 1):
        images, detected, gts = pack
        images = Variable(images)
        detected = Variable(detected)
        gts = Variable(gts)
        images = images.cuda(device)
        detected = detected.cuda(device)
        gts = gts.cuda(device)
        traindata = torch.cat([images,detected],1)
        labeldata = torch.cat([images,gts],1)

        model_optimizer.zero_grad()

        output = model(traindata)

# Binary
        if __BIN_LOSS__ == True:
            _, output_mask = torch.chunk(output, 2, dim=1)
            _, gt_mask = torch.chunk(labeldata, 2, dim=1)
            loss = binary_loss_func(output_mask, gts)
        else:     
            loss = mse_loss_func(output, labeldata)

        loss.backward()
        model_optimizer.step()
        total_loss += loss.item()

        if ((i+1) % 30 == 0):
            print('{} Epoch [{:03d}/{:03d}], Step [{:04d}/{:04d}], Loss: {:.4f}'.
                  format(datetime.now(), epoch, args.epoch, i, total_step, loss.data))
        
        if(best_loss>loss.data):
            best_loss = loss.data
            best_epoch = epoch
            pth_filename = save_path +'best_pth/'+'_step:%03d epoch:%03d loss: %.4f.pth'% (i, epoch, best_loss)
            torch.save(model.state_dict(), pth_filename)    # save_path +'best_pth/_step:'+'.%03d batch' %i +'_epoch:'+'.%03d' % epoch + ' loss2: %.4f'%opt.best_loss+'_w.pth')

            output_filename = save_path+'epoch_'+'%03d_iter_%03d-%.4f_result.png'%(epoch, i, loss.data)
            _, output_mask = torch.chunk(output, 2, dim=1)
            torchvision.utils.save_image(output_mask, output_filename)    #save_path+'epoch_'+str(epoch)+'_iter_'+str(i)+'{:.4f}_det_test.png'%loss2.data)
            gts_filename = save_path+'epoch_'+'%03d_iter_%03d-%.4f_GTs.png'%(epoch, i, loss.data)
            torchvision.utils.save_image(gts, gts_filename)    #save_path+'epoch_'+str(epoch)+'_iter_'+str(i)+'{:.4f}_det_test.png'%loss2.data)
            detected_filename = save_path+'epoch_'+'%03d_iter_%03d-%.4f_detected.png'%(epoch, i, loss.data)
            torchvision.utils.save_image(detected, detected_filename)    #save_path+'epoch_'+str(epoch)+'_iter_'+str(i)+'{:.4f}_det_test.png'%loss2.data)
            img_filename = save_path+'epoch_'+'%03d_iter_%03d-%.4f_img.png'%(epoch, i, loss.data)
            torchvision.utils.save_image(images, img_filename)    #save_path+'epoch_'+str(epoch)+'_iter_'+str(i)+'{:.4f}_det_test.png'%loss2.data)



'''
if _ % 400 ==0:
    print(i)
    print(loss)
    v_utils.save_image(x.cpu().data,"./result/original_image_{}_{}.png".format(i,_))
    v_utils.save_image(y_.cpu().data,"./result/label_image_{}_{}.png".format(i,_))
    v_utils.save_image(y.cpu().data,"./result/gen_image_{}_{}.png".format(i,_))
    torch.save(generator,'./model/{}.pkl'.format(args.network))    
'''    

