# DACON 컴퓨터 비전 학습 경진 대회

> 대회의 문제를 해결하기 위해서 진행된 EDA, Code, Submission의 설명 및 기록.

[DACON 컴퓨터 비전 학습 경진대회 Link.](https://dacon.io/competitions/open/235626/overview/description)

[DACON 컴퓨터 비전 학습 경진대회 EDA Summary.](https://www.notion.so/9233351a081340988f7343eed541aff7)

## Directory structure description
```컴퓨터 비전 학습 경진 대회
├── 참고자료 (Code 작성 시 참고한 파일)
├── Code(ipynb) (작성, 실험 결과기록을 위한 파일)
├── data (실험에 사용된 데이터)
├── EDA (EDA의 결과기록을 위한 파일)
└── Submission (DACON 컴퓨터 비전 학습 경진대회에 제출한 CSV 파일)
``` 

## Rules.
Public Score: 전체 테스트 데이터 중 1%로 채점

Private Score: 나머지 테스트 데이터로 채점

외부데이터 사용 불가, 사전학습모델 (pretrained model) 사용 불가

## Summary.

1. Data Argmentation의 필요성 판단 (CVLC_04를 통해서 진행 중)
> Data Argmentation을 적용한 것과 적용하지 않은 모델을 학습하여 결과를 비교할 예정.

2. No Data Argmentation, model and optimizer change.(CVLC_05를 통해서 확인. public-0.96078, private-0.90397) 
> 기존 Baseline에서 model, optimizer를 변경하였더니 81%에서 96%로 아주 높은 개선을 볼 수 있었다. 
>
> Result Link - [Baseline](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20(public-0.81862%2C%20private-0%2C76593).ipynb), [Baseline + model change](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20model%20change%20(public-0.90686%2C%20private-0.89687).ipynb), [Baseline + model change + optimizar change](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_05_baseline%20model%20change%20%2B%20optimizer%20change%20(public-0.96078%2C%20private-0.90397).ipynb)
>
> 해당 결과로 모델의 성능이 결과에 아주 큰 영향을 끼친다는 것을 판단하여, CVLC-07을 통해서 여러 좋은 모델들을 사용하게 된다면, 정확도를 더욱 향상 시킬 수 있을것이라고 생각되어 진행하였다.

3. Model change. [Keras model Link.](https://keras.io/ko/applications/) (CVLC_06을 통해서 진행 중)

|public| private |              모델 | 상위-1 정확성 | 상위-5 정확성 |    매개변수 | 깊이 | Result Link |
|:-----:|:-----:|------------------:|--------------:|--------------:|------------:|:----:|:---------:|
|  |      |             VGG16 |         0.713 |         0.901 | 138,357,544 |  23  |  |
| |      |             VGG19 |         0.713 |         0.900 | 143,667,240 |  26  | |
| |   |          ResNet50 |         0.749 |         0.921 |  25,636,712 |   -  | |
| 0.92857 | 0.90377   |         ResNet101 |         0.764 |         0.928 |  44,707,176 |   -  | [Link](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_06_ResNet101(public-0.92857%2C%20private-0.90377).ipynb) |
| |    |         ResNet152 |         0.766 |         0.931 |  60,419,944 |   -  | |
| |       |        ResNet50V2 |         0.760 |         0.930 |  25,613,800 |   -  | |
| |      |       ResNet101V2 |         0.772 |         0.938 |  44,675,560 |   -  | |
| |       |       ResNet152V2 |         0.780 |         0.942 |  60,380,648 |   -  | |
| |       |         ResNeXt50 |         0.777 |         0.938 |  25,097,128 |   -  | |
| |       |        ResNeXt101 |         0.787 |         0.943 |  44,315,560 |   -  | |
| |       |       InceptionV3 |         0.779 |         0.937 |  23,851,784 |  159 | |
| |       | InceptionResNetV2 |         0.803 |         0.953 |  55,873,736 |  572 | |
| |       |       DenseNet121 |         0.750 |         0.923 |   8,062,504 |  121 | |
| |       |       DenseNet169 |         0.762 |         0.932 |  14,307,880 |  169 | |
| |       |       DenseNet201 |         0.773 |         0.936 |  20,242,984 |  201 | |

4. 15개의 모델 예측 결과를 이용하여서 예측 빈도가 가장 많이 보이는 숫자를 사용. (CVLC_07을 통해서 확인. public-0.94607, private-0.93317)
> 3개의 model을 ensemble 한 결과 - public-0.94607, private-0.93090. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Three_model_ensemble(public-0.94607%2C%20private-0.93090).ipynb)
>
> 11개의 model을 ensemble 한 결과 - public-0.94607, private-0.93317로 private의 성능 향상을 보였다. [Result Link.](https://github.com/d9249/DACON/blob/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%EB%8C%80%ED%9A%8C/Code(ipynb)/CVLC_07_Eleven_model_ensemble(public-0.94607%2C%20private-0.93317).ipynb)

> 15개의 모델을 비교하려하였는데, VGG16, VGG19, ResNeXt50, ResNeXt101 4개의 model의 error가 있어서 학습을 중단하였으며,
> 기존의 사용하려했던 15개의 모델 중 11개의 모델로 충분히 여러개의 모델을 사용하여 결과를 내는 것에 대한 정확도 향상을 볼 수 있다고 판단하여, 11개의 모델의 ensemble 결과를 내었다.

> Found 1642 images belonging to 10 classes.
> 
> Found 406 images belonging to 10 classes.
> 
> Train 2048개를 1642개를 train data로 406개를 validation data로 나누어서 학습을 진행하였습니다.

| val_accuracy |              모델 | 상위-1 정확성 | 상위-5 정확성 |    매개변수 | 깊이 |
|:-----:|------------------:|--------------:|--------------:|------------:|:----:|
|   X   |             VGG16 |         0.713 |         0.901 | 138,357,544 |  23  |
|   X   |             VGG19 |         0.713 |         0.900 | 143,667,240 |  26  |
|  0.92857  |          ResNet50 |         0.749 |         0.921 |  25,636,712 |   -  |
|  0.93596  |         ResNet101 |         0.764 |         0.928 |  44,707,176 |   -  |
|  0.92857  |         ResNet152 |         0.766 |         0.931 |  60,419,944 |   -  |
|  0.93350     |        ResNet50V2 |         0.760 |         0.930 |  25,613,800 |   -  |
|  0.92118    |       ResNet101V2 |         0.772 |         0.938 |  44,675,560 |   -  |
|  0.93596     |       ResNet152V2 |         0.780 |         0.942 |  60,380,648 |   -  |
|    X   |         ResNeXt50 |         0.777 |         0.938 |  25,097,128 |   -  |
|    X   |        ResNeXt101 |         0.787 |         0.943 |  44,315,560 |   -  |
|  0.94581     |       InceptionV3 |         0.779 |         0.937 |  23,851,784 |  159 |
|  0.93103     | InceptionResNetV2 |         0.803 |         0.953 |  55,873,736 |  572 |
|  0.93842     |       DenseNet121 |         0.750 |         0.923 |   8,062,504 |  121 |
|  0.93596     |       DenseNet169 |         0.762 |         0.932 |  14,307,880 |  169 |
|  0.93103     |       DenseNet201 |         0.773 |         0.936 |  20,242,984 |  201 |

5. 모델의 최적화를 위한 Parameter 조정 
> 현재 학습은 weights를 설정하지 않고 학습하지만, 추후에 weights를 설정하여서 학습을 진행하게 될 경우 학습의 영향을 미칠 것으로 예상됨. 
>
> (CVLC_08을 통해서 진행 예정)
