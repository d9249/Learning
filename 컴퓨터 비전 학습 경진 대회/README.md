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

# Optimization.

Parameter optimization(ImageDataGenerator), Optimizer optimization, Data input size tendency를 통해서
나온 가장 최적화된 Parameter로 Individual model(After parameter optimization)을 진행하였습니다.

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

# Result.

Voting ensemble(Before parameter optimization)에서는 **0.93386**에 그쳤지만, 
Parameter optimization(ImageDataGenerator), Data input size tendency, Optimizer optimization을 진행한 결과 
private score **0.94599**을 달성하여, 해당 대회가 열린 기간의 점수 중 가장 높은 점수를 달성할 수 있었다.
