from django.shortcuts import render


def category(request):
    return render(request, 'category.html')


def createCategory(request):
    return render(request, 'newDataForm.html')


def categoryDetail(request):
    return render(request, 'categoryDetail.html')


def fileUpload(request):
    return render(request, 'fileUpload.html')


def detailCrack(request):
    return render(request, 'detailCrack.html')
