from django.shortcuts import render, redirect, get_object_or_404
from numpy import core, str_
from .models import Photo, Category
import numpy as np
import cv2
from django.core.paginator import Paginator
from django.http import JsonResponse, Http404
import json
from django.db.models import Q
from io import BytesIO
from PIL import Image
import re
import base64
from django.db.models import Q

# Create your views here.


def index(request):
    if request.method == 'POST':
        img = Photo()
        img.image = request.FILES.get('uploadImg')
        img.flatting_image = request.FILES.get('uploadImg')
        img.save()
        return redirect('info-upload/' + str(img.id))
    else:
        return render(request, 'index.html')


def infoUpload(request, pk):
    photo = get_object_or_404(Photo, pk=pk)
    category = Category.objects.all()
    return render(request, 'infoUpload.html', {
        'img': photo,
        'category': category,
    })


def infoProcess(request, pk):
    photo = Photo.objects.get(pk=pk)
    photo.originWidth = request.POST['width']
    photo.originHeight = request.POST['height']
    photo.state = request.POST['state']
    photo.cause = request.POST['cause']
    photo.solution = request.POST['solution']
    if request.POST['category'] != 'noselect':
        category = Category.objects.get(name=request.POST['category'])
        photo.category = category
    photo.save()
    return redirect('db')


def lengthCalc(request):
    if request.method == 'POST':
        pk = request.POST['pk']
        image = get_object_or_404(Photo, pk=pk)
        img = cv2.imread(image.image.url[1:])

        width = image.originWidth
        height = image.originHeight

        # 좌상 우상 우하 좌하
        topLeft = request.POST['x'].split(',')
        topRight = request.POST['y'].split(',')
        bottomRight = request.POST['w'].split(',')
        bottomLeft = request.POST['z'].split(',')

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
        dst = cv2.warpPerspective(img, M=M, dsize=(
            int(width_ratio*pixelHeight), int(height_ratio*pixelHeight)))
        cv2.imwrite(image.flatting_image.url[1:], dst)
        image.isFlattened = True
        image.save()

        return render(request, 'lengthCalc.html', {
            'img': image,
            'height': height,
            'imgWidth': int(width_ratio*pixelHeight),
            'imgHeight': int(height_ratio*pixelHeight),
        })


def db(request):
    dateList = Photo.objects.all().order_by('-id')
    page = request.GET.get('page', '1')
    paginator = Paginator(dateList, '5')  # Paginator(분할될 객체, 페이지 당 담길 객체수)
    page_obj = paginator.page(page)  # 페이지 번호를 받아 해당 페이지를 리턴 get_page 권장
    return render(request, 'db.html', {'page_obj': page_obj})


def categories(request):
    photos = Photo.objects.filter(~Q(category=None)).order_by('-id')
    categories = Category.objects.all().order_by('-id')
    categoryDic = []
    categoryList = ''
    for i in categories:
        temp = []
        temp.append(i.name)
        categoryList = i.name+' '+categoryList
        for j in photos:
            if i == j.category:
                temp.append(j)
        categoryDic.append(temp)
        print(categoryList)
    if request.method == 'GET':
        return render(request, 'categories.html', {'lists': categoryDic, 'categories': categoryList})
    else:
        objCategory = Category()
        objCategory.name = request.POST['newCategory']
        objCategory.save()
        return redirect('/categories')


def dbDetail(request, pk):
    try:
        photo = Photo.objects.get(pk=pk)
    except:
        raise Http404("해당 게시물을 찾을 수 없습니다.")
    return render(request, 'dbDetail.html', {
        'img': photo
    })


def search(request):
    if request.method == 'POST':
        searchStr = json.loads(request.body).get('searchText')
        expenses = Photo.objects.filter(category__name__icontains=searchStr)
        data = expenses.values()
        list_data = list(data)
        for i in range(len(expenses)):
            list_data[i]['category_name'] = expenses[i].category.name
        return JsonResponse(list_data, safe=False)


def flatting(request, pk):
    try:
        photo = Photo.objects.get(pk=pk)
    except:
        raise Http404("해당 게시물을 찾을 수 없습니다.")
    return render(request, 'flatting.html', {
        'img': photo
    })


def categoryDetail(request, name):
    try:
        category = Category.objects.get(name=name)
        photo = Photo.objects.filter(category=category)
    except:
        raise Http404("해당 게시물을 찾을 수 없습니다.")
    return render(request, 'categoryDetail.html', {
        'img': photo,
        'categoryName': category.name,
    })


def saveCanvas(request, pk):
    if request.method == 'POST':
        image = get_object_or_404(Photo, pk=pk)
        crackLength = json.loads(request.body).get("crackLength")
        dataURL = json.loads(request.body).get("dataURL")
        dataURL = re.sub("^data:image/png;base64,", "", dataURL)
        dataURL = base64.b64decode(dataURL)
        dataURL = BytesIO(dataURL)
        img = Image.open(dataURL)
        img = np.array(img)
        image.crackLength = crackLength
        image.save()
        cv2.imwrite(image.flatting_image.url[1:], img)
        return redirect("/db")
    else:
        return redirect("/")


def update(request, pk):
    if request.method == 'POST':
        photo = get_object_or_404(Photo, pk=pk)
        category = Category.objects.all()
        return render(request, 'infoUpdate.html', {
            'img': photo,
            'category': category,
        })
    else:
        return render(request, 'get.html')


def updatePost(request, pk):
    if request.method == 'POST':
        photo = Photo.objects.get(pk=pk)
        photo.originWidth = request.POST['width']
        photo.originHeight = request.POST['height']
        photo.state = request.POST['state']
        photo.cause = request.POST['cause']
        photo.solution = request.POST['solution']
        if request.POST['category'] != 'noselect':
            category = Category.objects.get(name=request.POST['category'])
            photo.category = category
        photo.save()
    else:
        return render(request, 'get.html')
    return redirect('db')


def deletePost(request, pk):
    if request.method == "POST":
        photo = Photo.objects.get(pk=pk)
        photo.delete()
        return redirect('db')
    else:
        return render(request, 'get.html')
