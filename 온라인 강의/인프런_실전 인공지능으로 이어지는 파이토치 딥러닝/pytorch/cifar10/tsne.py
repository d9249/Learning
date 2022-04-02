from sklearn.manifold import TSNE
import numpy as np
from matplotlib import pyplot as plt
import models
import torch
import torch.nn as nn

class Identity(nn.Module):
    def __init__(self):
        super(Identity, self).__init__()

    def forward(self, x):
        return x

def tsne(dataloader, model_name, pretrained):

    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
    model_name = model_name
    model = models.modeltype(model_name)
    model = model.to(device)
    model.load_state_dict(torch.load(pretrained))
    model.fc = Identity()

    actual = []
    deep_features = []

    model.eval()
    with torch.no_grad():
        for data in dataloader:
            images, labels = data[0].to(device), data[1].to(device)
            features = model(images)

            deep_features += features.cpu().numpy().tolist()
            actual += labels.cpu().numpy().tolist()

    tSNE = TSNE(n_components=2, random_state=0)
    cluster = np.array(tSNE.fit_transform(np.array(deep_features)))
    actual = np.array(actual)

    plt.figure(figsize=(10, 10))
    cifar = ['plane', 'car', 'bird', 'cat', 'deer', 'dog', 'frog', 'horse', 'ship', 'truck']
    for i, label in zip(range(10), cifar):
        idx = np.where(actual == i)
        plt.scatter(cluster[idx, 0], cluster[idx, 1], marker='.', label=label)

    plt.legend()
    plt.savefig('./results/'+ model_name+'_tSNE.png')
