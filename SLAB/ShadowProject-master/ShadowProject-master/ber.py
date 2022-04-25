import numpy as np
#import cv2 as cv
from matplotlib import pyplot as plt
import imageio
from PIL import Image
from io import BytesIO
from ber_data_loader import *
import torch.utils.data as Data

def BalancedErrorRate(original, imageGT):
    TP=0
    FN=0
    FP=0
    TN=0
    e =0 
    for k in range(original.shape[0]):
        for m in range(original.shape[1]):
            if(np.any(original[k,m])== 1 and np.any(imageGT[k,m]) == 1):
                TP=TP+1
                e = e+1
            elif(np.any(original[k,m])==1 and np.any(imageGT[k,m]) == 0):
                FP=FP+1
                e = e+1
            elif(np.any(original[k,m]) == 0 and np.any(imageGT[k,m]) == 1):
                FN=FN+1
                e = e+1
            elif(np.any(original[k,m]) ==0 and np.any(imageGT[k,m]) == 0):
                TN=TN+1
                e = e+1
    
    SHADOW = (float(TP)/float(TP+FN))
    NONSHADOW = (float(TN)/float(TN+FP))
    BER = 1.0-((SHADOW+NONSHADOW)/2.0)
    return BER,SHADOW,NONSHADOW


#device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
dataset = shadow_triplets_loader()
data_loader = Data.DataLoader(dataset,batch_size = 1)
total =0
total_shadow =0
total_NONSHADOW = 0
for x_,y_ in enumerate(data_loader) : 
    inference_mask, shadow_mask = y_
    inference_mask = np.array(inference_mask, dtype = float)
    shadow_mask = np.array(shadow_mask, dtype = float)
    inference_mask=inference_mask.squeeze()
    shadow_mask = shadow_mask.squeeze()
    
    BER,SHADOW,NONSHADOW = BalancedErrorRate(inference_mask,shadow_mask)
    total = total + BER
    total_shadow = total_shadow + (1-SHADOW)
    total_NONSHADOW = total_NONSHADOW + (1-NONSHADOW)
    
    print("total_shadow = %f"%(total_shadow))
    print("total_NONSHADOW = %f"%(total_NONSHADOW))
    print("total_ber = %f"%(total))
    
print("BER_total_a = %f"%(total/400))#data_size = 400
print("BER_total_shadow = %f"%(total_shadow/400))
print("BER_total_NONSHADOW = %f"%(total_NONSHADOW/400))