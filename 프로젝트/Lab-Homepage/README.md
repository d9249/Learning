# Smart IoT Lab Website - v2.0.1
경기대학교 네트워크 연구실 <b>Smart IoT Lab</b> 웹사이트

## New Features
- 정적 데이터 추가
  - 졸업한 연구생 정보 및 사진 추가
- Publication 준비중
- 연구실 홈페이지 도메인이 변경됨
  - `https://uns.kyonggi.ac.kr` -> `https://netlab.kyonggi.ac.kr`

## Maintenance
배포를 하는 것은 코드의 개발 및 테스트가 완료되었으며, 배포할 모든 준비가 되어 개발사항들을 `main` 브런치에 병합하는 것을 의미한다.
1. 모든 개발사항들을 `develop` 브런치에 push(또는 merge) 한다.
2. `develop` 브런치에서 다시 브런치를 나눈다. 이름은 v2.0.2과 같이 한다.
3. `main` 브런치에 병합하기 전에 최종적으로 커밋을 확인하며 `README`를 작성한다.
4. `main` 브런치에 병합한다.

GitHub에 푸시한 개발사항들을 연구실 서버에 업데이트 한다.

1. ssh 접속 및 패키지 업데이트
```
ssh nslab@netlab.kyonggi.ac.kr -p 8087
nslab@netlab.kyonggi.ac.kr's password: [패스워드 입력]

sudo yum update
```

2. git pull
```
cd WEB/Lab-Homepage
git pull
```

3. 프론트엔드 배포
```
cd frontend
npm install && npm run build
```
```
# nginx가 접근중인 경로는 `/var/www/dist`이며 이미 존재하는 디렉터리를 삭제한 이후에 새로 build한 디렉터리를 옮긴다.

sudo rm -r /var/www/dist && sudo mv dist /var/www
```

4. 벡엔드 배포
```
# (선택)
# 필요하다면 개발 시 사용했던 config 데이터들을 복사한다.
# scp는 ssh와 유사하지만 파일을 복사하는 명령어이다. -P 옵션은 포트를 설정한다.
# -r 옵션은 디렉터리를 복사할 때 사용한다.
# 아래 명령어는 config 디렉터리를 backend/server에 복사하는 예시이다.

scp -P 8087 -r config/ nslab@netlab.kyonggi.ac.kr:/home/nslab/WEB/Lab-Homepage/backend/server
```

```
cd ../backend
npm install && npm run build
```

```
# pm2는 api 서버를 무중단 서비스 하기 위해 사용한다.
# 환경변수 NODE_ENV를 production으로 설정한다.
# ssl 파일에 접근하기 위해 sudo가 필요하다.
# pm2에서 npm 명령어를 사용하기 위해 아래와 같이 실행시킨다. 

sudo NODE_ENV=production pm2 start npm -- start
```

## Contributors
- 장민호 [minho-jang](https://github.com/minho-jang)
- 이상민 [iDeal](https://github.com/d9249)


<br>

Copyright &copy; 2021 by Smart IoT Lab <br>
All rights reserved. 
