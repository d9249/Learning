import os
from PIL import Image
import torch.utils.data as data
import torchvision.transforms as transforms

TO_GRAY = False

class SalObjDataset(data.Dataset):
    def __init__(self, image_root,height_root, gt_root, trainsize, inputchannel):
        self.trainsize = trainsize
        self.images = [image_root + f for f in os.listdir(image_root) if f.endswith('.png')]
        self.height = [height_root + f for f in os.listdir(height_root) if f.endswith('.png')]
        self.gts = [gt_root + f for f in os.listdir(gt_root) if f.endswith('.png')
                    or f.endswith('.png')]
        self.images = sorted(self.images)
        self.height = sorted(self.height)
        self.gts = sorted(self.gts)
        #self.filter_files()
        self.size = len(self.images)
        if inputchannel == 4:
            self.img_transform = transforms.Compose([
                transforms.Resize((self.trainsize, self.trainsize)),
                transforms.ToTensor(),
                transforms.Normalize([0.485, 0.456, 0.406], [0.229, 0.224, 0.225])])
        else:   # input image : gray
            self.img_transform = transforms.Compose([
                transforms.Resize((self.trainsize, self.trainsize)),
                transforms.Grayscale(num_output_channels=1), 
                transforms.ToTensor(),
                transforms.Normalize(mean=[0.5], std=[0.5])])
        self.height_transform = transforms.Compose([
            transforms.Resize((self.trainsize, self.trainsize)),
            transforms.ToTensor()])
        self.gt_transform = transforms.Compose([
            transforms.Resize((self.trainsize, self.trainsize)),
            transforms.ToTensor()])

    def __getitem__(self, index):
        image = self.rgb_loader(self.images[index])
        height = self.binary_loader(self.height[index])
        gt = self.binary_loader(self.gts[index])
        image = self.img_transform(image)
        height = self.height_transform(height)
        gt = self.gt_transform(gt)
        return image, height,gt

    def filter_files(self):
        assert len(self.images) == len(self.gts)
        images = []
        gts = []
        for img_path, gt_path in zip(self.images, self.gts):
            img = Image.open(img_path)
            gt = Image.open(gt_path)
            if img.size == gt.size:
                images.append(img_path)
                gts.append(gt_path)
        self.images = images
        self.gts = gts

    def rgb_loader(self, path):
        with open(path, 'rb') as f:
            img = Image.open(f)
            return img.convert('RGB')

    def binary_loader(self, path):
        with open(path, 'rb') as f:
            img = Image.open(f)
            # return img.convert('1')
            return img.convert('L')

    def resize(self, img, gt):
        assert img.size == gt.size
        w, h = img.size
        if h < self.trainsize or w < self.trainsize:
            h = max(h, self.trainsize)
            w = max(w, self.trainsize)
            return img.resize((w, h), Image.BILINEAR), gt.resize((w, h), Image.NEAREST)
        else:
            return img, gt

    def __len__(self):
        return self.size


def get_loader(image_root,height_root ,gt_root, batchsize, trainsize, shuffle=True, num_workers=12, pin_memory=True, inputchannel=4):
    dataset = SalObjDataset(image_root,height_root, gt_root, trainsize, inputchannel)
    data_loader = data.DataLoader(dataset=dataset, batch_size=batchsize)

    return data_loader


class test_dataset:
    def __init__(self, image_root, mask_root,height_root, testsize, inputchannel):
        self.testsize = testsize
        self.images = [image_root + f for f in os.listdir(image_root) if f.endswith('.png')]
        self.masks = [mask_root + f for f in os.listdir(mask_root) if f.endswith('.png')]
        self.heights = [height_root + f for f in os.listdir(height_root) if f.endswith('.png')
                       or f.endswith('.jpg')]
        self.images = sorted(self.images)
        self.masks = sorted(self.masks)
        self.heights = sorted(self.heights)
        if inputchannel == 4:
            self.transform = transforms.Compose([
                transforms.Resize((self.testsize, self.testsize)),
                transforms.ToTensor(),
                transforms.Normalize([0.485, 0.456, 0.406], [0.229, 0.224, 0.225])])
        else:   # input image : gray
            self.transform = transforms.Compose([
                transforms.Resize((self.testsize, self.testsize)),
                transforms.Grayscale(num_output_channels=1), 
                transforms.ToTensor(),
                transforms.Normalize(mean=[0.5], std=[0.5])])

        self.gt_transform = transforms.Compose([
            transforms.Resize((self.testsize,self.testsize)),
            transforms.ToTensor()])
        self.size = len(self.images)
        self.index = 0

    def load_data(self):
        image = self.rgb_loader(self.images[self.index])
        image = self.transform(image).unsqueeze(0)
        mask = self.binary_loader(self.masks[self.index])
        height = self.binary_loader(self.heights[self.index])
        height = self.gt_transform(height).unsqueeze(0)
        
        
        #height = self.transform(height).unsqueeze(0)
        
        name = self.images[self.index].split('/')[-1]
        if name.endswith('.png'):
            name = name.split('.png')[0] + '.png'
        self.index += 1
        return image, mask,height, name

    def rgb_loader(self, path):
        with open(path, 'rb') as f:
            img = Image.open(f)
            return img.convert('RGB')

    def binary_loader(self, path):
        with open(path, 'rb') as f:
            img = Image.open(f)
            return img.convert('L')


