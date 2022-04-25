# import requests
import time
import telegram
import urllib.request
from bs4 import BeautifulSoup

bot = telegram.Bot(token='5144106058:AAGfnmn8wVpIyjq7nmwbTxEf7Zw78KLY230')
chatBot_id = -123456789

if __name__ == '__main__':
    try:
        print("경기대 일반대학원 공지사항")
        changeList = None
        currentList = None
        sendList = None
        changeList = []
        currentList = []
        resultList = []
        
        while True:
            url = "http://www.kyonggi.ac.kr/boardList.kgu?bcode=B0039&mzcode=K0301M0400&orgCd=K0301"
            req = urllib.request.Request(url)
            sourcecode = urllib.request.urlopen(url).read()
            soup = BeautifulSoup(sourcecode, "html.parser")
            
            noticesCrop = soup.find("table", {"class": "basic"})
            links = noticesCrop.find_all("a")
        
            for a in links:
                href = a.attrs['href']
                currentList.append("www.kyonggi.ac.kr"+href)  # 현재 들어온거

            time.sleep(3) 

            # changeList = currentList
            # if changeList != currentList:
                # resultList = changeList - currentList

            print(currentList)
            # print("change : ", changeList)
            # print(currentList)
            # print("result : ", resultList)

    except Exception as es:
        print("오류 발생")
        # bot.sendMessage('bot 확인 요망.')