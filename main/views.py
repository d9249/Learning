from calendar import IllegalMonthError
from django.shortcuts import get_object_or_404, render, redirect

from main.facility import facility,looks

from .models import Category, Crack, CrackObj
#-- 이미지 변환 --#
from io import BytesIO
import json
import re
import base64
from PIL import Image
#--------------#

#-- 평탄화 --#
import numpy as np
import cv2
#----------#
from openpyxl import Workbook
from openpyxl.styles import Alignment
from openpyxl.cell.cell import ILLEGAL_CHARACTERS_RE

import os

import mimetypes
from django.http.response import HttpResponse
import urllib.parse


def index(request):
    return render(request, 'index.html')

def error(request):
    return render(request, 'error.html')


def category(request):
    categorys = Category.objects.all()
    return render(request, 'category.html', {
        'categorys': categorys
    })


def input(request):
    if request.method == 'POST':
        # page 1
        name = request.POST['name']
        number = request.POST['number']
        location = request.POST['location']
        date = request.POST['date']
        site_area = request.POST['site-area']
        building_area = request.POST['building-area']
        year_area = request.POST['year-area']
        max_height = request.POST['max-height']
        use = request.POST['use']
        structure = request.POST['structure']
        format = request.POST['format']
        facility = request.POST['facility']
        floor = request.POST['floor']
        grade = request.POST['grade']
        result = request.POST['result']
        plus = request.POST['plus']

        # page 2
        locationMap = request.FILES['locationMap']
        frontView = request.FILES['frontView']

        # create object
        category = Category()
        category.facilityName = name
        category.facilityNo = number
        category.address = location
        category.completionDate = date
        category.landArea = site_area
        category.buildingArea = building_area
        category.totalBuildingArea = year_area
        category.highestHeight = max_height
        category.usage = use
        category.facilityStructure = structure
        category.structuralForm = format
        category.amenities = facility
        category.floors = floor
        category.grade = grade
        category.testResults = result
        category.plus = plus
        category.frontView = frontView
        category.locationMap = locationMap
        category.save()
        return redirect('categoryDetail/'+str(category.id))
    return render(request, 'input.html')


def categoryList(request, pk):
    category = Category.objects.get(pk=pk)
    cracks = Crack.objects.filter(category=category)
    return render(request, 'categoryList.html', {
        'cracks': cracks
    })


def categoryDetail(request, pk):
    category = Category.objects.get(pk=pk)
    crack = Crack.objects.filter(category=category)
    return render(request, 'categoryDetail.html',{'obj':category,'crack':crack})

def buildingDetail(request, pk):
    category = Category.objects.get(pk=pk)
    crack = Crack.objects.filter(category=category)
    return render(request, 'buildingDetail.html',{'obj':category,'crack':crack})   


def flatting(request, pk):
    crack = CrackObj.objects.get(pk=pk)
    return render(request, 'flatting.html', {
        'crack': crack
    })


def flattingResult(request):
    if request.method == 'POST':
        pk = request.POST['pk']
        crack = get_object_or_404(CrackObj, pk=pk)
        temp = cv2.imread(crack.image.url[1:])
        width = crack.originWidth        
        height = crack.originHeight

        topLeft = request.POST['tl'].split(',')
        topRight = request.POST['tr'].split(',')
        bottomLeft = request.POST['bl'].split(',')
        bottomRight = request.POST['br'].split(',')

        pts1 = np.float32([
            [int(int(topLeft[0])), int(int(topLeft[1]))],
            [int(int(topRight[0])), int(int(topRight[1]))],
            [int(int(bottomRight[0])), int(int(bottomRight[1]))],
            [int(int(bottomLeft[0])), int(int(bottomLeft[1]))]
        ])

        pixelHeight = max(np.linalg.norm(
            pts1[0] - pts1[3]), np.linalg.norm(pts1[1] - pts1[2]))
        width_ratio = width/height
        height_ratio = 1

        pts2 = np.array([
            [0, 0],
            [int(width_ratio*pixelHeight), 0],
            [int(width_ratio*pixelHeight), int(height_ratio*pixelHeight)],
            [0, int(height_ratio*pixelHeight)]
        ], dtype=np.float32)

        M = cv2.getPerspectiveTransform(pts1, pts2)
        dst = cv2.warpPerspective(temp, M=M, dsize=(
            int(width_ratio*pixelHeight), int(height_ratio*pixelHeight)))

        cv2.imwrite(crack.flatting_image.url[1:], dst)
        crack.isFlattened = True
        crack.save()
        if (request.POST['result'] == 'length'):
            return render(request, 'flattingResult.html', {
                'crack': crack,
                'height': height,
                'imgWidth': int(width_ratio*pixelHeight),
                'imgHeight': int(height_ratio*pixelHeight),
            })
        else :
            return render(request, 'areaResult.html',{
                'crack': crack,
                'width' : width,
                'height': height,
                
            })


def area(request):
    return render(request, 'area.html')


def createCrack(request, pk):
    if request.method == 'POST':
        crack = Crack()
        crack.floor = request.POST['floor']
        crack.location = request.POST['location']
        crack.crackType = request.POST['crackType']
        crack.place = request.POST['place']
        crack.cause = request.POST['cause']
        crack.note = request.POST['note']
        crack.category = Category.objects.get(pk=pk)
        crack.save()
        return redirect('/categoryDetail/'+str(pk))
    return render(request, 'createCrack.html',{'objData':{'pk':pk}})

def crackDetail(request,pk):
    crack = Crack.objects.get(pk=pk)
    crackObj = CrackObj.objects.filter(parent=crack)
    return render(request, 'crackDetail.html', {'crackData':{'pk':pk},'crackObj': crackObj})


def createCrackObj(request,pk):
    if request.method == 'POST':
        crack = Crack.objects.get(pk=pk)
        crackObj = CrackObj()
        crackObj.image = request.FILES['image']
        crackObj.flatting_image = request.FILES['image']
        crackObj.date = request.POST['date']
        crackObj.originWidth = request.POST['width']
        crackObj.originHeight = request.POST['height']
        crackObj.parent = crack
        crackObj.save()
        print(crack)
        crack.date = request.POST['date']
        crack.save()
        return redirect('/crackDetail/'+str(pk))

    return render(request,'createCrackObj.html',{'crackData':{'pk':pk}})


def save(request, pk):
    if request.method == "POST":
        crack = get_object_or_404(CrackObj, pk=pk)
        print('crack', crack)
        crackLength = json.loads(request.body).get("crackLength")
        dataURL = json.loads(request.body).get("dataURL")
        dataURL = re.sub("^data:image/png;base64,", "", dataURL)
        dataURL = base64.b64decode(dataURL)
        dataURL = BytesIO(dataURL)
        temp = Image.open(dataURL)
        temp = np.array(temp)
        crack.crackLength = crackLength
        crack.save()
        parent = crack.parent
        parent.crackSize = 'L='+crackLength+'mm'
        parent.save()
        cv2.imwrite(crack.flatting_image.url[1:], temp)
        return redirect('category')
    else:
        return render(request, 'error.html')

def saveArea(request,pk):
    if request.method == "POST":
        crack = get_object_or_404(CrackObj, pk=pk)
        crackArea = json.loads(request.body).get("crackArea")
        dataURL = json.loads(request.body).get("dataURL")
        dataURL = re.sub("^data:image/png;base64,", "", dataURL)
        dataURL = base64.b64decode(dataURL)
        dataURL = BytesIO(dataURL)
        temp = Image.open(dataURL)
        temp = np.array(temp)
        crack.crackArea = crackArea
        crack.save()
        parent = crack.parent
        parent.crackSize = 'A='+crackArea+'㎡'
        parent.save()
        cv2.imwrite(crack.flatting_image.url[1:], temp)
        return redirect('/crackDetail/'+ str(crack.parent.id))
    else:
        return render(request, 'error.html')

def createExcel(request,pk):
    category = Category.objects.get(pk=pk)
    crack = Crack.objects.filter(category=category)
    lists = [['구분','위치1','위치2','결함종류','결함규모','폭','개소','적출년도', '발생원인','비고']]
    div = 1
    for obj in crack:
        list=[div,obj.floor, obj.location, obj.crackType,obj.crackSize,obj.crackWidth,obj.place, obj.date,obj.cause,obj.note]
        div+=1
        lists.append(list)
        
    wb = Workbook()
    ws3 = wb.create_sheet("손상현황표",2)
    num = 1
    for list in lists:
        alp = 65
        ws3.column_dimensions['A']
        for i in list:
            ws3[chr(alp)+str(num)] = i
            ws3[chr(alp)+str(num)].alignment = Alignment(horizontal='center',vertical='bottom')
            alp+=1
        num+=1
    for column in range(65,75):
        ws3.column_dimensions[chr(column)].bestFit = True
    ws3.sheet_view.view = "pageBreakPreview"
    wb = facility(wb,pk)
    wb = looks(wb,pk)
    BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    fileName = category.facilityName + '.xlsx'
    filePath = BASE_DIR+"/media/excel/" + fileName
    wb.save(filePath)
    path = open(filePath, 'rb')
    mime_type, _ = mimetypes.guess_type(filePath)
    response = HttpResponse(path, content_type=mime_type)
    filename_header = 'filename*=UTF-8\'\'%s' % urllib.parse.quote(fileName.encode('utf-8'))
    response['Content-Disposition'] = "attachment;" + filename_header
    
    return response
    



def deleteCrackObj(request,pk):
    obj = CrackObj.objects.get(pk=pk)
    obj.delete()
    return redirect('/crackDetail/'+str(obj.parent.id))

def updateCrack(request, pk):
    crack = Crack.objects.get(pk=pk)
    print(crack)
    return render(request, 'updateCrack.html', {'crack':crack} )

def deleteCrack(request, pk):
    crack = Crack.objects.get(pk=pk)
    crack.delete()
    return redirect('/categoryDetail/'+str(crack.category.id))

def handleUpdateCrack(request,pk):
    crack = Crack.objects.get(pk=pk)
    crack.floor = request.POST['floor']
    crack.location = request.POST['location']
    crack.crackType = request.POST['crackType']
    crack.place = request.POST['place']
    crack.cause = request.POST['cause']
    crack.note = request.POST['note']
    crack.save()

    return redirect('/categoryDetail/'+str(crack.category.id))

def updateCategory(request,pk):
    category = Category.objects.get(pk=pk)
    return render(request,'updateCategory.html',{'category':category})

def handleUpdateCategory(request, pk):
    category = Category.objects.get(pk=pk)
    category.facilityName = request.POST['name']
    category.facilityNo = request.POST['number']
    category.address = request.POST['location']
    category.completionDate = request.POST['date']
    category.landArea = request.POST['site-area']
    category.buildingArea = request.POST['building-area']
    category.totalBuildingArea = request.POST['year-area']
    category.highestHeight = request.POST['max-height']
    category.usage = request.POST['use']
    category.facilityStructure = request.POST['structure']
    category.structuralForm = request.POST['format']
    category.amenities = request.POST['facility']
    category.floors = request.POST['floor']
    category.grade = request.POST['grade']
    category.testResults = request.POST['result']
    category.plus = request.POST['plus']
    category.frontView = request.FILES['frontView']
    category.locationMap = request.FILES['locationMap']
    category.save()
    return redirect('category')

def deleteCategory(request,pk):
    category = Category.objects.get(pk=pk)
    category.delete()
    return redirect('category')

def searchResult(request):
    keyword = request.POST['keyword']
    results = Category.objects.filter(facilityName__icontains = keyword)
    return render(request,'searchResult.html', {'results':results})