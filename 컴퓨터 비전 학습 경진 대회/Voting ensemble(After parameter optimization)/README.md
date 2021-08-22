## Voting ensemble
> Train data set 개를 개를 train data로 개를 validation data로 나누어서 학습을 진행하였으며,
>
> ImageDataGenerator을 사용하여서 Data Argmentation을 진행한 결과 학습에는
>
> Train image = , Validataion image = 가 사용되었습니다. 
>
> Individual model(After parameter optimization)을 통해서 개별적으로 학습된 모델을 이용하여서 Voting ensemble 하였다.

### Voting ensemble - 11 models

| Model                 | Private acc | Result Link | Default input size | Input size |
| --------------------- | ----------- | ----------- | ------------------ | ---------- |
| [VGG16]()             |             | [Link]()    | 224x224            | 224x224    |
| [VGG19]()             |             | [Link]()    | 224x224            | 224x224    |
| [ResNet50]()          |             | [Link]()    | 224x224            | 224x224    |
| [ResNet101]()         |             | [Link]()    | 224x224            | 224x224    |
| [ResNet152]()         |             | [Link]()    | 224x224            | 224x224    |
| [ResNet50V2]()        |             | [Link]()    | 224x224            | 224x224    |
| [ResNet101V2]()       |             | [Link]()    | 224x224            | 224x224    |
| [ResNet152V2]()       |             | [Link]()    | 224x224            | 224x224    |
| [InceptionV3]()       |             | [Link]()    | 299x299            | 224x224    |
| [InceptionResNetV2]() |             | [Link]()    | 299x299            | 224x224    |
| [DenseNet121]()       |             | [Link]()    | 224x224            | 224x224    |
| [DenseNet169]()       |             | [Link]()    | 224x224            | 224x224    |
| [DenseNet201]()       |             | [Link]()    | 224x224            | 224x224    |

> 11개의 model을 ensemble 한 결과 - public-, private-로 private의 성능 향상을 보였다. [Result Link.]()

### Voting ensemble - 25 models

| Model                 | Private acc | Result Link | Default input size | Input size |
| --------------------- | ----------- | ----------- | ------------------ | ---------- |
| [VGG16]()             |             | [Link]()    | 224x224            | 224x224    |
| [VGG19]()             |             | [Link]()    | 224x224            | 224x224    |
| [ResNet50]()          |             | [Link]()    | 224x224            | 224x224    |
| [ResNet101]()         |             | [Link]()    | 224x224            | 224x224    |
| [ResNet152]()         |             | [Link]()    | 224x224            | 224x224    |
| [ResNet50V2]()        |             | [Link]()    | 224x224            | 224x224    |
| [ResNet101V2]()       |             | [Link]()    | 224x224            | 224x224    |
| [ResNet152V2]()       |             | [Link]()    | 224x224            | 224x224    |
| [InceptionV3]()       |             | [Link]()    | 299x299            | 224x224    |
| [InceptionResNetV2]() |             | [Link]()    | 299x299            | 224x224    |
| [InceptionV3]()       |             | [Link]()    | 299x299            | 299x299    |
| [InceptionResNetV2]() |             | [Link]()    | 299x299            | 299x299    |
| [DenseNet121]()       |             | [Link]()    | 224x224            | 224x224    |
| [DenseNet169]()       |             | [Link]()    | 224x224            | 224x224    |
| [DenseNet201]()       |             | [Link]()    | 224x224            | 224x224    |
| [Xception]()          |             | [Link]()    | 299x299            | 224x224    |
| [Xception]()          |             | [Link]()    | 299x299            | 299x299    |
| [EfficientNetB0]()    |             | [Link]()    | 224x224            | 224x224    |
| [EfficientNetB1]()    |             | [Link]()    | 240x240            | 224x224    |
| [EfficientNetB1]()    |             | [Link]()    | 240x240            | 240x240    |
| [EfficientNetB2]()    |             | [Link]()    | 260x260            | 224x224    |
| [EfficientNetB2]()    |             | [Link]()    | 260x260            | 260x260    |
| [EfficientNetB3]()    |             | [Link]()    | 300x300            | 224x224    |
| [EfficientNetB3]()    |             | [Link]()    | 300x300            | 300x300    |
| [EfficientNetB4]()    |             | [Link]()    | 380x380            | 224x224    |
| EfficientNetB4        | X           |             | 380x380            | 380x380    |
| [EfficientNetB5]()    |             | [Link]()    | 456x456            | 224x224    |
| EfficientNetB5        | X           |             | 456x456            | 456x456    |
| [EfficientNetB6]()    |             | [Link]()    | 528x528            | 224x224    |
| EfficientNetB6        | X           |             | 528x528            | 528x528    |
| EfficientNetB7        | X           |             | 600x600            | 224x224    |
| EfficientNetB7        | X           |             | 600x600            | 600x600    |

> 25개의 model을 voting ensemble 한 결과 - public-, private-로 private의 성능 향상을 보였다. [Result Link.]()
