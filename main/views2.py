from django.http.response import Http404
from django.shortcuts import get_object_or_404, render, redirect
from .models import Category, Crack
import numpy as np
import cv2


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
        print(category)
        return render(request, 'input.html')
    return render(request, 'input.html')


def save(request):
    return redirect("/")


def categoryList(request, pk):
    category = Category.objects.get(pk=pk)
    cracks = Crack.objects.filter(category=category)
    return render(request, 'categoryList.html', {
        'cracks': cracks
    })


def categoryDetail(request):
    return render(request, 'categoryDetail.html')


def flatting(request, pk):
    crack = Crack.objects.get(pk=pk)
    return render(request, 'flatting.html', {
        'crack': crack
    })


def flattingResult(request):
    if request.method == 'POST':
        pk = request.POST['pk']
        crack = get_object_or_404(Crack, pk=pk)
        print(crack)
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

        return render(request, 'flattingResult.html', {
            'crack': crack,
            'height': height,
            'imgWidth': int(width_ratio*pixelHeight),
            'imgHeight': int(height_ratio*pixelHeight),
        })


def area(request):
    return render(request, 'area.html')
