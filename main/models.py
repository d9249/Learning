from django.db import models

# Create your models here.


class Category(models.Model):
    facilityName = models.CharField(
        null=False, max_length=255, unique=True)  # 시설물 명
    facilityNo = models.CharField(null=True, max_length=255)  # 시설물 번호
    address = models.CharField(null=True, max_length=255)  # 시설물 주소
    completionDate = models.DateField(null=True)  # 준공일자
    landArea = models.CharField(null=True, max_length=255)  # 대지면적
    buildingArea = models.CharField(null=True, max_length=255)  # 건축면적
    totalBuildingArea = models.CharField(null=True, max_length=255)  # 건축연면적
    highestHeight = models.CharField(null=True, max_length=255)  # 최고높이
    usage = models.CharField(null=True, max_length=255)  # 용도
    facilityStructure = models.CharField(null=True, max_length=255)  # 시설물 구조
    structuralForm = models.CharField(null=True, max_length=255)  # 구조형식 영어
    amenities = models.CharField(null=True, max_length=255)  # 부대시설
    floors = models.CharField(null=True, max_length=255)  # 층별
    grade = models.CharField(null=True, max_length=255)  # 전차안전등급
    testResults = models.CharField(null=True, max_length=255)  # 점검결과
    plus = models.CharField(null=True, max_length=255)  # 규모 및 추가 사항
    frontView = models.ImageField(
        upload_to='images/frontView/%Y/%m/%d/', null=True)
    locationMap = models.ImageField(
        upload_to='images/locationMap/%Y/%m/%d/', null=True)

    def __str__(self):
        return str(self.facilityName)


class Crack(models.Model):
    
    floor = models.CharField(null=True, max_length=255)  # 층수
    location = models.CharField(null=True, max_length=255)  # 위치
    absence = models.CharField(null=True, max_length=255)  # 부재
    desc = models.CharField(null=True, max_length=255)  # 점검내용
    place = models.CharField(null=True, max_length=255)  # 개소
    number = models.CharField(null=True, max_length=255)  # 현황번호
    progress = models.CharField(null=True, max_length=255)  # 진행유무
    cause = models.CharField(null=True, max_length=255)  # 발생원인
    note = models.CharField(null=True, max_length=255)  # 비고
    category = models.ForeignKey(
        'Category', on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id)


class CrackObj(models.Model):
    image = models.ImageField(upload_to='images/crack_img/origin/%Y/%m/%d/', null=False, blank=False)
    flatting_image = models.ImageField(upload_to='images/crack_img/flatting/%Y/%m/%d/', null=True)
    isFlattened = models.BooleanField(default=False)
    originWidth = models.FloatField(null=True)
    originHeight = models.FloatField(null=True)
    crackLength = models.FloatField(null=True)
    date = models.DateField(null=True)
    parent = models.ForeignKey('Crack', on_delete=models.CASCADE)


    def __str__(self):
        return str(self.id)