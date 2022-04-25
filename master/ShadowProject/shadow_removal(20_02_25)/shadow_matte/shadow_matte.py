import numpy as np
from matplotlib import pyplot as plt
import imageio
from PIL import Image
from io import BytesIO
from data_loader1 import *
import torch.utils.data as Data
import torch

def shadow_matte(x,original_image):
  x = torch.clamp(x, 1e-3, 255)
  original_image = torch.clamp(original_image, 1e-3, 255)
  log_shadow_matte = torch.log(x)- torch.log(original_image)
  log_shadow_matte = torch.clamp(log_shadow_matte,-float('Inf'),torch.log(torch.tensor(255.0)))
  
  return log_shadow_matte

dataset = shadow_triplets_loader()
data_loader = Data.DataLoader(dataset,batch_size = 1)

folder_path = './test_shadow_matte/'
if not os.path.exists(folder_path):
  os.makedirs(folder_path)

for x_,y_ in enumerate(data_loader) :
    original_image, shadow_mask, shadow_free,name = y_
    torchvision.utils.save_image(shadow_matte(shadow_mask,shadow_free), folder_path+name[0])