from django.db import models

# Create your models here.

class Category(models.Model):
  facilityName = models.CharField(null=True, max_length=255, unique=True)  # 시설물 명
  facilityNo = models.CharField(null=True, max_length=255)# 시설물 번호
  adress = models.CharField(null=True, max_length=255)# 시설물 주소
  completionDate =  models.DateField(null=True)# 준공일자
  landArea = models.CharField(null=True, max_length=255)# 대지면적
  buildingArea = models.CharField(null=True, max_length=255)# 건축면적
  totalBuildingArea = models.CharField(null=True, max_length=255)# 건축연면적
  highestHeight = models.CharField(null=True, max_length=255)# 최고높이
  Usage = models.CharField(null=True, max_length=255)# 용도
  facilityStructure = models.CharField(null=True, max_length=255)# 시설물 구조
  structuralForm = models.CharField(null=True, max_length=255)# 구조형식 영어
  amenities = models.CharField(null=True, max_length=255)# 부대시설
  floors = models.CharField(null=True, max_length=255)# 층별
  grade = models.CharField(null=True, max_length=255)# 전차안전등급
  testResults = models.CharField(null=True, max_length=255)# 점검결과
  plus = models.CharField(null=True, max_length=255)# 규모 및 추가 사항
  frontView = models.ImageField(upload_to='images/frontView/%Y/%m/%d/', null=True)
  locationMap = models.ImageField(upload_to='images/locationMap/%Y/%m/%d/', null=True)
  def __str__(self):
        return str(self.id)