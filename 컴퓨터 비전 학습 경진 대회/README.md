# [컴퓨터 비전 학습 경진 대회.](https://dacon.io/competitions/open/235626/overview/description)

> 대회의 문제를 해결하기 위해서 진행된 EDA, Code, Submission의 설명 및 기록.

# Directory structure description.
```컴퓨터 비전 학습 경진 대회
├── Baseline(DACON)
├── Data Argmentation
├── Data visualization
├── Data(DACON)
├── Docs
├── EDA(Exploratory Data Analysis)
├── Image
├── Individual model(After parameter optimization)
├── Individual model(Before parameter optimization)
├── Optimizer optimization
├── Parameter optimization(ImageDataGenerator)
├── Submission
├── Voting ensemble(After parameter optimization)
└── Voting ensemble(Before parameter optimization)
```

# Rules.
Public Score: 전체 테스트 데이터 중 1%로 채점

Private Score: 나머지 테스트 데이터로 채점

외부데이터 사용 불가, 사전학습모델 (pretrained model) 사용 불가

## Optimization.

[Parameter optimization(ImageDataGenerator)](https://github.com/d9249/DACON/tree/main/컴퓨터 비전 학습 경진 대회/Parameter optimization(ImageDataGenerator)), [Optimizer optimization](https://github.com/d9249/DACON/tree/main/컴퓨터 비전 학습 경진 대회/Optimizer optimization), [Data input size tendency](https://github.com/d9249/DACON/tree/main/컴퓨터 비전 학습 경진 대회/Data input size tendency) 를 통해서
나온 가장 최적화된 Parameter로 [**Individual model(After parameter optimization)**](https://github.com/d9249/DACON/tree/main/%EC%BB%B4%ED%93%A8%ED%84%B0%20%EB%B9%84%EC%A0%84%20%ED%95%99%EC%8A%B5%20%EA%B2%BD%EC%A7%84%20%EB%8C%80%ED%9A%8C/Individual%20model(After%20parameter%20optimization)) 을 진행하였습니다.

```
image size = 224.
Batch_size = 8.
optimizer = Adam.
epochs = 500.
ImageDataGenerator (
		rescale = 1./255, 
		validation_split = 0.075,
		rotation_range = 15,
		width_shift_range = 0.00,
		height_shift_range = 0.05 )
```

