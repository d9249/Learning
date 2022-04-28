# Image Stride and Patch Size 별로 Crop.

이미지 파일을 원하는 stride와 patch size를 입력받아 분할하기 위한 tool. 

(Tools for truncating image files into stride and patch sizes.)

실행 예 : Dataset과 같이 crop.py 을 실행한 전체 디렉토리 구조는 다음과 같이 되어있다.
```bash
├── input
│   ├── image
│   └── mask
├── output
│   └── [filename]_[stride]_[patch_x_size]_[patch_y_size]
│        ├── [filename]_[stride]_[patch_x_size]_[patch_y_size]_image
│        └── [filename]_[stride]_[patch_x_size]_[patch_y_size]_mask
├── crop.py
└── Parameter.txt
``` 

디렉토리 구조에서 실행 시 아래의 예시대로 Parameter.txt 파일을 읽어들여 실행된다.

Example. Parameter.txt
```
patch_x_size = 64
patch_y_size = 48
stride = 10
input_dir_image = input/image # 분할 대상 이미지의 경로
input_dir_mask = input/mask # 분할 대상 이미지의 경로
output_dir = output # 분할된 이미지의 결과물이 저장될 경로
```
원하는 Patch size와 stride, 대상이 되는 데이터셋이 있는 디렉토리 경로에 있는 이미지를 대상으로 

Crop된 이미지를 사용자가 작성한 output_directory에 해당하는 디렉토리를  생성 후 저장한다.

위의 Parameter.txt 파일의 대상 파일의 경로, stride, patch size를 읽어들여 Crop된 이미지를 output_dir에 선언된 곳에 생성한다.

해당되는 데이터셋 중 mask는 그림자 매트에 해당하며 전체 이미지 white 화소수를 파일명에 추가로 저장한다.

구조 예시 : Output 디렉토리 구조
```bash
├── [output] 
│   └── [filename]_[stride]_[patch_x_size]_[patch_y_size]
│       └── [filename]_[stride]_[patch_x_size]_[patch_y_size]_image
│       │  └── [filename]_[stride]_[patch_x_size]_[patch_y_size]_[col_num]_[row_num]_image.[Original filename extension]
│       └── [filename]_[stride]_[patch_x_size]_[patch_y_size]_mask
│            └── [filename]_[stride]_[patch_x_size]_[patch_y_size]_[col_num]_[row_num]_mask_[num_white_pixel].[Original filename extension]
...
``` 

작동 예시 : output folder.
```bash
├── output
│   └── 1_10_64_48
│        └── 1_10_64_48_image
│        │   └── 1_10_64_48_0_0_image.jpg
│        │                                   ,,,
│        └── 1_10_64_48_mask
│              ├── 1_10_64_48_0_0_mask_0.jpg
│              │                             ,,,
│              └── 1_10_64_48_15_45_mask_1307.jpg
│                                         ,,,
...
``` 
