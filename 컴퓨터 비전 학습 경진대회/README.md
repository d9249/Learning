# DACON

DIYA Computer Vision 4기에서 컴퓨터 비전 관련 프로젝트를 진행, 기록하기 위한 레포지토리 입니다.

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

## To do list
1. Data Argmentation의 필요성 판단 (CVLC_04를 통해서 진행 중)
2. No Data Argmentation, model and optimizer change.(CVLC_05를 통해서 확인. public-0.96078, private-0.90397)
> 기존 Baseline에서 model, optimizer를 변경하였더니 81%에서 96%로 아주 높은 개선을 볼 수 있었다. 
3. Model change. [Keras model Link.](https://keras.io/ko/applications/) (CVLC_06을 통해서 진행 중)
4. 15개의 모델 예측 결과를 이용하여서 예측 빈도가 가장 많이 보이는 숫자를 사용. (CVLC_07을 통해서 확인. public-0.94607, private-0.93317)
> 3개의 모델을 앙상블 한 결과 - public-0.94607, private-0.93090.
> 11개의 모델을 앙상블 한 결과 - public-0.94607, private-0.93317로 private의 성능 향상을 보였다.

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

4. 모델의 최적화를 위한 Parameter 조정 
> 현재 학습은 weights를 설정하지 않고 학습하지만, 추후에 weights를 설정하여서 학습을 진행하게 될 경우 학습의 영향을 미칠 것으로 예상됨. (CVLC_08을 통해서 진행 예정)
