# DACON 컴퓨터 비전 학습 경진 대회

> 대회의 문제를 해결하기 위해서 진행된 EDA, Code, Submission의 설명 및 기록.

[DACON 컴퓨터 비전 학습 경진대회 Link.](https://dacon.io/competitions/open/235626/overview/description)

[DACON 컴퓨터 비전 학습 경진대회 EDA Summary.](https://www.notion.so/9233351a081340988f7343eed541aff7)

## Directory structure description
```컴퓨터 비전 학습 경진 대회
├── Code(ipynb) (작성, 실험 결과기록을 위한 파일)
├── EDA (EDA의 결과기록을 위한 파일)
├── Problematic code(ipynb) (문제가 있는 코드의 기록을 위한 파일) 
├── References (Code 작성 시 참고한 파일) 
├── Submission (DACON 컴퓨터 비전 학습 경진대회에 제출한 CSV 파일)
└── data (실험에 사용된 데이터) 
``` 

## Rules.
Public Score: 전체 테스트 데이터 중 1%로 채점

Private Score: 나머지 테스트 데이터로 채점

외부데이터 사용 불가, 사전학습모델 (pretrained model) 사용 불가

## Summary.

1. Data Argmentation의 필요성 판단 (CVLC_04를 통해서 진행 중)
> Data Argmentation을 적용한 것과 적용하지 않은 모델을 학습하여 결과를 비교할 예정.

2. No Data Argmentation, model and optimizer change.(CVLC_05를 통해서 확인. public-0.96078, private-0.90397) 
> 기존 Baseline에서 model을 변경하였더니, 81%에서 90%의 9%의 향상을 보였고 
> 
> 추가적으로 optimizer의 세부 parameter를 변경하였더니 90%에서 96%로 6%의 정확도 향상을 보였다. 
>
> Result Link - [Baseline](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20(public-0.81862%2C%20private-0%2C76593).ipynb), [Baseline + model change](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20model%20change%20(public-0.90686%2C%20private-0.89687).ipynb), [Baseline + model, optimizar change](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20model%20change%20%2B%20optimizer%20change%20(public-0.96078%2C%20private-0.90397).ipynb)
>
> 해당 결과로 모델의 성능이 결과에 아주 큰 영향을 끼친다는 것을 판단하여, CVLC-07을 통해서 여러 좋은 모델들을 사용하게 된다면, 
> 
> 정확도를 더욱 향상 시킬 수 있을것이라고 생각되어 진행하였다.

![image](https://user-images.githubusercontent.com/60354713/128587385-446cc2bf-e60c-4957-82c3-a68dcf961985.png)
![image](https://user-images.githubusercontent.com/60354713/128587390-c5aea0bc-df91-4eeb-b762-dffb24f3ff86.png)

3.  Single model을 사용 하여서 개별 모델들의 정확도 판단. [tensorflow keras application model Link.](https://www.tensorflow.org/api_docs/python/tf/keras/applications), [Keras applications documentation](https://keras.io/ko/applications/)

기존의 tensorflow for keras application을 사용하였는데, 아래의 도표는 keras documentation을 참고하여서 작성하였기에 Top-1 acc, Top-5 acc, Parameter, Depth를 추가적으로 다시 알아보아서 수정해야될 것 같습니다.

> (CVLC_06을 통해서 확인, 아래의 도표에 모델 마다의 정확도를 나타내었습니다.)

> Found 1642 images belonging to 10 classes.
> 
> Found 406 images belonging to 10 classes.
> 
> Train data set 2048개를 1642개를 train data로 406개를 validation data로 나누어서 학습을 진행하였습니다.

| Public accuracy| Private accuracy| Model (Link : Experiment Results File)| Top-1 accuracy | Top-5 accuracy | Parameter | Depth | Result Link | Default Input Size | Input Size |
|:-----:|:-----:|------------------:|--------------:|--------------:|------------:|:----:|:---------:|:----:|:---------:|
| X |  X   |             VGG16 |         0.713 |         0.901 | 138,357,544 |  23  |  | 224x224 | 224x224|
| X |  X    |             VGG19 |         0.713 |         0.900 | 143,667,240 |  26  | | 224x224 | 224x224|
| 0.93137 | 0.90930  |          [ResNet50](https://drive.google.com/file/d/1k8xoKPKdJW1fTpYT1gtHEfRItyuKb0AT/view?usp=sharing) |         0.749 |         0.921 |  25,636,712 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50(public-0.93137%2C%20private-0.90930).ipynb) | 224x224 | 224x224 |
| 0.92857 | 0.90377   |         [ResNet101](https://drive.google.com/file/d/1L98_1aydEzZuRPfl7CYadLJ9I6K_eM55/view?usp=sharing) |         0.764 |         0.928 |  44,707,176 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101(public-0.92857%2C%20private-0.90377).ipynb) | 224x224 | 224x224 |
| 0.90196 | 0.89568   |         [ResNet152](https://drive.google.com/file/d/1PSrJRKc4dd8R2FGhifNOku3JkYsl-Ikb/view?usp=sharing) |         0.766 |         0.931 |  60,419,944 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152(public-0.90196%2C%20private-0.89568).ipynb) | 224x224 | 224x224 |
| 0.89215 |   0.90076    |        [ResNet50V2](https://drive.google.com/file/d/1qv9lv15CQFarucihFBw0eI6BigjXu-gh/view?usp=sharing) |         0.760 |         0.930 |  25,613,800 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet50V2(public-0.89215%2C%20private-0.90076).ipynb) | 224x224 | 224x224 |
| 0.91666 |   0.91512   |       [ResNet101V2](https://drive.google.com/file/d/1jh8OaHg1DFLbZLJxygQeMvU1nVRlaHvb/view?usp=sharing) |         0.772 |         0.938 |  44,675,560 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101V2(public-0.91666%2C%20private-0.91512).ipynb) | 224x224 | 224x224 |
| 0.89705 |    0.89647   |       [ResNet152V2](https://drive.google.com/file/d/1yOFCoBasy-Gtn438QiPMA_BHs9JBtY9d/view?usp=sharing) |         0.780 |         0.942 |  60,380,648 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet152V2(public-0.89705%2C%20private-0.89647).ipynb) | 224x224 | 224x224 |
| 0.92156 |  0.91640     |       [InceptionV3](https://drive.google.com/file/d/1aBePi6eqdXHpqnxMSRIyZZK3LrZy3kTo/view?usp=sharing) |         0.779 |         0.937 |  23,851,784 |  159 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(public-0.92156%2C%20private-0.91640).ipynb) | 299x299 | 224x224 |
| 0.94117 | 0.91408      | [InceptionResNetV2](https://drive.google.com/file/d/15PbssO2ZdUrtpL1iKTiCTGThM9rHxt7p/view?usp=sharing) |         0.803 |         0.953 |  55,873,736 |  572 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(public-0.94117%2C%20private-0.91408).ipynb) | 299x299 | 224x224 |
| 0.81862 |   0.82831    |       [InceptionV3](https://drive.google.com/file/d/1_oc49fIH1YZP4-8bzM9KwmdSWjP6nnYL/view?usp=sharing) |         0.779 |         0.937 |  23,851,784 |  159 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionV3(Input%20shape-299x299%2C%20public-0.81862%2C%20private-0.82831).ipynb) | 299x299 | 299x299 |
|  0.73039 | 0.74758      | [InceptionResNetV2](https://drive.google.com/file/d/1zhL8x-GgwapwlTJ2g2KtjdZKkZ0Ey1CQ/view?usp=sharing) |         0.803 |         0.953 |  55,873,736 |  572 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_InceptionResNetV2(Input%20shape%20299x299%2C%20public-0.73039%2C%20private-0.74758).ipynb) | 299x299 | 299x299 |
| 0.93137 |  0.91689     |       [DenseNet121](https://drive.google.com/file/d/1RTz47GS80clxxCi7y8G__Pd4KGbKgzia/view?usp=sharing) |         0.750 |         0.923 |   8,062,504 |  121 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet121(public-0.93137%2C%20private-0.91689).ipynb) | 224x224 | 224x224 |
| 0.92156 | 0.91285      |       [DenseNet169](https://drive.google.com/file/d/15kS7_mohTv6xVvr84Of1-H36ql0CjnLe/view?usp=sharing) |         0.762 |         0.932 |  14,307,880 |  169 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet169(public-0.92156%2C%20private-0.91285).ipynb) | 224x224 | 224x224 |
| 0.91666 | 0.90940      |       [DenseNet201](https://drive.google.com/file/d/1A-tkg-SWwoN1WvGicnEH_nPTWr764wzC/view?usp=sharing) |         0.773 |         0.936 |  20,242,984 |  201 | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_DenseNet201(public-0.91666%2C%20private-0.90940).ipynb) | 224x224 | 224x224 |
| X |  X    |             [Xception]() |         0.790 |         0.945 | 22,910,480	 |  126  | [Link]() | 299x299 | 224x224|
| X |  X    |             [Xception]() |         0.790 |         0.945 | 22,910,480	 |  126  | [Link]() | 299x299 | 299x299|
| X |  X    |             [EfficientNetB0]() |         0 |         0 |  |  0  | [Link]() | 224x224 | 224x224|
| X |  X    |             [EfficientNetB1]() |         0 |         0 |  |  0  | [Link]() | 240x240 | 224x224|
| X |  X    |             [EfficientNetB2]() |         0 |         0 |  |  0  | [Link]() | 260x260 | 224x224|
| X |  X    |             [EfficientNetB3]() |         0 |         0 |  |  0  | [Link]() | 300x300 | 224x224|
| X |  X    |             [EfficientNetB4]() |         0 |         0 |  |  0  | [Link]() | 380x380 | 224x224|
| X |  X    |             [EfficientNetB5]() |         0 |         0 |  |  0  | [Link]() | 456x456 | 224x224|
| X |  X    |             [EfficientNetB6]() |         0 |         0 |  |  0  | [Link]() | 528x528 | 224x224|
| X |  X    |             [EfficientNetB7]() |         0 |         0 |  |  0  | [Link]() | 600x600 | 224x224|

![image](https://user-images.githubusercontent.com/60354713/128587328-808800c1-0c97-49db-b28f-27052eb9db16.png)
![image](https://user-images.githubusercontent.com/60354713/128587357-7b4bf933-287d-40b7-974e-fecd8c1aa71f.png)
![image](https://user-images.githubusercontent.com/60354713/128607279-6377d969-32fe-4008-a4d0-49232a3b447a.png)
![image](https://user-images.githubusercontent.com/60354713/129208821-578df996-1032-4d9f-805c-d2fa2e6718b9.png)

4. 15개의 모델 예측 결과를 이용하여서 예측 빈도가 가장 많이 보이는 숫자를 사용. 
> (CVLC_07을 통해서 확인. public-0.94607, private-0.93317)

> Found 1642 images belonging to 10 classes.
> 
> Found 406 images belonging to 10 classes.
> 
> Train data set 2048개를 1642개를 train data로 406개를 validation data로 나누어서 학습을 진행하였습니다.

> 3개의 model을 ensemble 한 결과 - public-0.94607, private-0.93090. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Three_model_ensemble(public-0.94607%2C%20private-0.93090).ipynb)
>
> 11개의 model을 ensemble 한 결과 - public-0.94607, private-0.93317로 private의 성능 향상을 보였다. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Eleven_model_ensemble(public-0.94607%2C%20private-0.93317).ipynb)

> 15개의 모델을 비교하려하였는데, VGG16, VGG19, ResNeXt50, ResNeXt101 4개의 model running error가 있어서 학습을 중단하였으며,
> 
> 기존의 사용하려했던 15개의 모델 중 11개의 모델로 충분히 여러개의 모델을 사용하여 결과를 내는 것에 대한 
> 
> 정확도 향상을 볼 수 있다고 판단하여, 11개의 모델의 ensemble 결과를 내었다.

| val_accuracy |              Model | Top-1 accuracy | Top-5 accuracy |    Parameter | Depth |
|:-----:|------------------:|--------------:|--------------:|------------:|:----:|
|   X   |             VGG16 |         0.713 |         0.901 | 138,357,544 |  23  |
|   X   |             VGG19 |         0.713 |         0.900 | 143,667,240 |  26  |
|  0.92857  |          ResNet50 |         0.749 |         0.921 |  25,636,712 |   -  |
|  0.93596  |         ResNet101 |         0.764 |         0.928 |  44,707,176 |   -  |
|  0.92857  |         ResNet152 |         0.766 |         0.931 |  60,419,944 |   -  |
|  0.93350     |        ResNet50V2 |         0.760 |         0.930 |  25,613,800 |   -  |
|  0.92118    |       ResNet101V2 |         0.772 |         0.938 |  44,675,560 |   -  |
|  0.93596     |       ResNet152V2 |         0.780 |         0.942 |  60,380,648 |   -  |
|  0.94581     |       InceptionV3 |         0.779 |         0.937 |  23,851,784 |  159 |
|  0.93103     | InceptionResNetV2 |         0.803 |         0.953 |  55,873,736 |  572 |
|  0.93842     |       DenseNet121 |         0.750 |         0.923 |   8,062,504 |  121 |
|  0.93596     |       DenseNet169 |         0.762 |         0.932 |  14,307,880 |  169 |
|  0.93103     |       DenseNet201 |         0.773 |         0.936 |  20,242,984 |  201 |

![image](https://user-images.githubusercontent.com/60354713/128587281-2a2cf2be-b2ee-4139-aaf5-c715bebb2744.png)

5. 모델의 더 높은 정확성를 위한 pre-training weight 사용.
> 현재 학습은 weights를 설정하지 않고 학습하지만, 추후에 weights를 설정하여서 학습을 진행하게 될 경우 학습의 영향을 미칠 것으로 예상됨. 
>
> (CVLC_08을 통해서 진행 예정이였으나, pre-training on ImageNet된 모델의 가중치를 불러다 쓰기 때문에 대회의 규칙상 pre-training을 사용하면 안되기 때문에 취소하였다.)

6. optimizer의 세부 parameter를 조정하지않고, 기존의 있던 optimizer를 불러서 학습을 진행하여 정확도의 차이를 볼 예정.
> CVLC_09를 통해서 진행 예정.
