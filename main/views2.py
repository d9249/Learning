from django.shortcuts import render


def category(request):
    return render(request, 'category.html')


def createCategory(request):
    return render(request, 'newDataForm.html')


def categoryList(request):
    return render(request, 'categoryList.html')


def categoryDetail(request):
    return render(request, 'categoryDetail.html')


def fileUpload(request):
    return render(request, 'fileUpload.html')


def detailCrack(request):
    return render(request, 'detailCrack.html')


def flatting(request):
    return render(request, 'flatting.html')


def flattingResult(request):
    return render(request, 'flattingResult.html')


def area(request):
    return render(request, 'area.html')
