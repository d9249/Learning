import os, csv, json, torch, PIL

from PIL import Image
from torch.utils.data import Dataset
from sklearn.model_selection import train_test_split


# 엄지 붙인 약속을 '약속_'로 변경
csv_file = './hand_gesture_pose.csv'
label2info, info2label = {}, {}
with open(csv_file, 'r') as c:
    temp = csv.reader(c)
    for i, row in enumerate(temp):
        if i == 0: continue
        if int(row[0]) in [49, 74, 124, 149, 174]: row[2] = '약속_'
        label2info[int(row[0])] = tuple(row[-4:-1])
        info2label[tuple(row[-4:-1])] = int(row[0])

data_dir = './train/'
imgs, hands, lrbs = [], [], []
for num in sorted(os.listdir(data_dir)):
    with open(data_dir + '{}/{}.json'.format(num, num), 'r') as j:
        temp = json.load(j)
        for info in temp['annotations']:
            imgs.append(data_dir + '{}/{}.png'.format(num, info['image_id']))
            hands.append(label2info[temp['action'][0]][1])
            lrbs.append(label2info[temp['action'][0]][2])
            
crops, meanings = [], []
for num in sorted(os.listdir(data_dir)):
    with open(data_dir + '{}/{}.json'.format(num, num), 'r') as j:
        temp = json.load(j)
        for info in temp['annotations']:
            img_dir = data_dir + '{}/{}.png'.format(num, info['image_id'])
            raw_img = Image.open(img_dir).convert('RGB')
            
            coords = torch.tensor(info['data'])[:21, :-1]
            max_x, max_y = torch.max(coords, dim=0)[0]
            min_x, min_y = torch.min(coords, dim=0)[0]
            max_x, max_y, min_x, min_y = int(max_x), int(max_y), int(min_x), int(min_y)
                
            max_x, min_x = min(1920, max_x + 50), max(0, min_x - 150)
            max_y, min_y = min(1080, max_y + 100), max(0, min_y - 100)
            
            crop_img = raw_img.crop((min_x, min_y, max_x, max_y))
            
            w, h = max_x-min_x, max_y-min_y
            if w > h:
                padded_img = Image.new(crop_img.mode, (w, w), (0, 0, 0))
                padded_img.paste(crop_img, (0, (w-h)//2))
            else:
                padded_img = Image.new(crop_img.mode, (h, h), (0, 0, 0))
                padded_img.paste(crop_img, ((h-w)//2, 0))
            
            crops.append(padded_img)
            meanings.append(label2info[temp['action'][0]][0])


meaning2num = {'OK': 0, 'X': 1, '검지,중지 교차': 2, '경고(주먹 내밀기)': 3, '꼬집기': 4, '네모': 5,
               '동그라미': 6, '부정(검지 흔들기)': 7, '부정(엄지손 아래로)': 8, '빅토리': 9, '세모': 10, 
               '손 안경': 11, '손 토끼': 12, '손가락 오므리기': 13, '손가락 접기': 14, '손바닥': 15, '손하트': 16,
               '숫자 0': 17, '숫자0': 17,
               '숫자 1': 18, '숫자1': 18,
               '숫자 2': 19, '숫자2': 19,
               '숫자 3': 20, '숫자3': 20, 
               '숫자 4': 21, '숫자4': 21,
               '숫자 5': 22, '숫자5': 22,
               '숫자 6': 23, '숫자6': 23,
               '숫자 7': 24, '숫자7': 24,
               '숫자 8': 25, '숫자8': 25,
               '숫자 9': 26, '숫자9': 26,
               '약속': 27, '약속_': 28, 
               '약지,소지 가위': 29, '엄지,소지 붙이기': 30, '엄지,약지 붙이기': 31, '엄지,중지 붙이기': 32,
               '전화모양': 33, '주먹쥐기': 34, '총': 35, '최고': 36, '큰하트': 37, '파이팅': 38}
num2meaning = {}
for meaning in meaning2num.keys(): num2meaning[meaning2num[meaning]] = meaning

hand2num = {label:i for i, label in enumerate(sorted(set(hands)))}
num2hand = {i:label for i, label in enumerate(sorted(set(hands)))}

lrb2num = {label:i for i, label in enumerate(sorted(set(lrbs)))}
num2lrb = {i:label for i, label in enumerate(sorted(set(lrbs)))}
            

train_mimgs, val_mimgs, train_mlabels, val_mlabels = train_test_split(crops, meanings, random_state=0, stratify=meanings)
train_himgs, val_himgs, train_hlabels, val_hlabels = train_test_split(imgs, hands, random_state=0, stratify=hands)
train_limgs, val_limgs, train_llabels, val_llabels = train_test_split(imgs, lrbs, random_state=0, stratify=lrbs)


class MDataset(Dataset):
    def __init__(self, data_type='train', transform=None):
        if data_type == 'train':
            self.imgs = train_mimgs
            self.labels = train_mlabels
        elif data_type == 'val':
            self.imgs = val_mimgs
            self.labels = val_mlabels
        
        self.meaning2num = meaning2num
        self.num2meaning = num2meaning
        self.transform = transform

    def __getitem__(self, idx):
        img = self.imgs[idx]
        if self.transform: img = self.transform(img)

        label = self.meaning2num[self.labels[idx]]
        return img, label

    def __len__(self):
        return len(self.imgs)

    
class HDataset(Dataset):
    def __init__(self, data_type='train', transform=None):
        if data_type == 'train':
            self.imgs = train_himgs
            self.labels = train_hlabels
        elif data_type == 'val':
            self.imgs = val_himgs
            self.labels = val_hlabels
        
        self.hand2num = hand2num
        self.num2hand = num2hand
        self.transform = transform

    def __getitem__(self, idx):
        img = PIL.Image.open(self.imgs[idx]).convert('RGB')
        if self.transform: img = self.transform(img)

        label = self.hand2num[self.labels[idx]]
        return img, label

    def __len__(self):
        return len(self.imgs)
    
    
class LDataset(Dataset):
    def __init__(self, data_type='train', transform=None):
        if data_type == 'train':
            self.imgs = train_limgs
            self.labels = train_llabels
        elif data_type == 'val':
            self.imgs = val_limgs
            self.labels = val_llabels
        
        self.lrb2num = lrb2num
        self.num2lrb = num2lrb
        self.transform = transform

    def __getitem__(self, idx):
        img = PIL.Image.open(self.imgs[idx]).convert('RGB')
        label = self.lrb2num[self.labels[idx]]
        if self.transform: img, label = self.transform(img, label)

        return img, label

    def __len__(self):
        return len(self.imgs)