from django.shortcuts import render


def category(request):
    return render(request, 'category.html')


def categoryDetail(request):
    return render(request, 'categoryDetail.html')
