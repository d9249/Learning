import torch, torchvision
import torchvision.transforms.functional as F

from PIL import Image, ImageOps


class Compose:
    def __init__(self, transforms):
        self.transforms = transforms

    def __call__(self, image, label):
        for t in self.transforms:
            image, label = t(image, label)
        return image, label
    
    
class Resize(torch.nn.Module):
    def __init__(self, size, interpolation=F.InterpolationMode.BILINEAR):
        super().__init__()
        self.size = size
        self.interpolation = interpolation

    def forward(self, image, label):
        return F.resize(image, self.size, self.interpolation), label
    
    
class RandomCrop(torch.nn.Module):
    @staticmethod
    def get_params(img, output_size):
        w, h = F._get_image_size(img)
        th, tw = output_size

        if w == tw and h == th:
            return 0, 0, h, w

        i = torch.randint(0, h - th + 1, size=(1, )).item()
        j = torch.randint(0, w - tw + 1, size=(1, )).item()
        return i, j, th, tw

    def __init__(self, size):
        super().__init__()
        self.size = size

    def forward(self, image, label):
        width, height = F._get_image_size(image)
        i, j, h, w = self.get_params(image, self.size)
        return F.crop(image, i, j, h, w), label
    
    
class CenterCrop(torch.nn.Module):
    def __init__(self, size):
        super().__init__()
        self.size = size

    def forward(self, image, label):
        return F.center_crop(image, self.size), label
    
    
class RandomHorizontalFlip(torch.nn.Module):
    def __init__(self, p=0.5):
        super().__init__()
        self.p = p
        
    def forward(self, image, label):
        if torch.rand(1) < self.p:
            image = F.hflip(image)
            # 'left': 1, 'right': 2
            if label == 1: label = 2
            elif label == 2: label = 1
        return image, label

    
class RandomRotation(torch.nn.Module):
    def __init__(self, degrees, interpolation=F.InterpolationMode.NEAREST, fill=0):
        super().__init__()
        self.degrees = [-degrees, degrees]
        self.resample = self.interpolation = interpolation

        if fill is None: fill = 0
        self.fill = fill

    @staticmethod
    def get_params(degrees):
        angle = float(torch.empty(1).uniform_(float(degrees[0]), float(degrees[1])).item())
        return angle

    def forward(self, image, label):
        angle = self.get_params(self.degrees)
        return F.rotate(image, angle, self.resample, False, None, self.fill), label
    
    
class ToTensor:
    def __call__(self, image, label):
        return F.to_tensor(image), label
    

class Normalize(torch.nn.Module):
    def __init__(self, mean, std, inplace=False):
        super().__init__()
        self.mean = mean
        self.std = std
        self.inplace = inplace

    def forward(self, tensor, label):
        return F.normalize(tensor, self.mean, self.std, self.inplace), label