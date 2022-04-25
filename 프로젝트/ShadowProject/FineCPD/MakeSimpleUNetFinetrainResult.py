import torchvision
from model.FusionNet import * 
from model.SimpleUNet import *
import numpy as np
import matplotlib.pyplot as plt
from datetime import datetime
from fine_data import get_loader
from fine_data import test_dataset
import os
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("--network",type=str,default="fusionnet",help="choose between fusionnet & unet")
parser.add_argument('--epoch', type=int, default=100, help='epoch number')
parser.add_argument("--batch_size",type=int,default=12,help="batch size")
parser.add_argument('--train_size', type=int, default=400, help='training image size')
parser.add_argument("--num_gpu",type=int,default=1,help="number of gpus")           # DON'T USE parallel
parser.add_argument('--foldername', type=str, default="", help='foldername')
parser.add_argument('--cudaid', type=int, default=1, help='GPU id')
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
model_path = '/home/juwan/SharedFolder/MyGitHub/FINECPD_BY_WANI/Learning/Layer3MSE10.14-09.44/best_pth/_step:270 epoch:048 loss: 0.0002.pth'
save_path =  '/home/juwan/SharedFolder/MyGitHub/FINECPD_BY_WANI/Learning/Layer3MSE10.14-09.44/SaveTrained/'
try:
    model.load_state_dict(torch.load(model_path))
#   model = torch.load('./model/{}.pkl'.format(args.network))
    print("\n--------model restored--------\n")
except:
    print("\n--------model not restored--------\n")
    pass



model.eval()

test_loader = test_dataset(image_root, detected_root, height_root, 400, 2)

for i in range(test_loader.size):
    image, mask, height, name = test_loader.load_data()
    mask = np.asarray(mask, np.float32)
    mask /= (mask.max() + 1e-8)
    image = torch.cat([image,height],1)
    image = image.cuda(device)
    _, res = model(image)
#        res = F.upsample(res, size=mask.shape, mode='bilinear', align_corners=False)
    res = res.sigmoid().data.cpu().numpy().squeeze()
    res = (res - res.min()) / (res.max() - res.min() + 1e-8)
    res = res>0.5
    #torchvision.utils.save_image(res,'result(no_height)/'+name+'.png')
    misc.imsave(save_path+name, res*255.0)
    print("save iamge", save_path+name)

