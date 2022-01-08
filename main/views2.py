from django.db import models
from django.shortcuts import render, redirect

from .models import Category

def category(request):
    return render(request, 'category.html')


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


def categoryList(request):
    return render(request, 'categoryList.html')


def categoryDetail(request):
    return render(request, 'categoryDetail.html')

def flatting(request):
    return render(request, 'flatting.html')

def flattingResult(request):
    return render(request, 'flattingResult.html')


def area(request):
    return render(request, 'area.html')
