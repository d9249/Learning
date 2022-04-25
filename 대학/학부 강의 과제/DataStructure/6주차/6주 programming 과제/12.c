#include <stdio.h>
#include <stdlib.h>

struct node
{
    int data;
    struct node *prev;
    struct node *next;
};

struct node *list = NULL;
struct node *list_last = NULL;

struct node *even = NULL;
struct node *even_last = NULL;

struct node *odd = NULL;
struct node *odd_last = NULL;

struct node *current = NULL;

void insert(int data)
{
    struct node *link = (struct node*) malloc(sizeof(struct node));

    link->data = data;
    link->prev = NULL;
    link->next = NULL;

    if(data%2 == 0)
    {
        if(even == NULL)
        {
            even = link;
            return;
        }
        else
        {
            current = even;

            while(current->next != NULL)
                current = current->next;

            current->next = link;
            even_last = link;
            link->prev = current;
        }
    }
    else
    {
        if(odd == NULL)
        {
            odd = link;
            return;
        }
        else
        {
            current = odd;

            while(current->next!=NULL)
                current = current->next;

            current->next = link;
            odd_last = link;
            link->prev = current;
        }
    }
}

void print_backward(struct node *head)
{
    struct node *ptr = head;

    printf("\n [last] <=>");
    while(ptr != NULL)
    {
        printf(" %d <=> ",ptr->data);
        ptr = ptr->prev;
    }

    printf(" [head] \n");
}

void printList(struct node *head)
{
    struct node *ptr = head;

    printf("\n [head] <=> ");
    while(ptr != NULL)
    {
        printf(" %d <=> ",ptr->data);
        ptr = ptr->next;
    }

    printf(" [last] \n");
}

void combine()
{
    struct node *link;

    list = even;
    link = list;

    while(link->next!= NULL)
    {
        link = link->next;
    }

    link->next = odd;
    odd->prev = link;

    while(link->next!= NULL)
    {
        link = link->next;
    }

    list_last = link;
}

int main()
{
    int i;

    for(i = 1; i <= 10; i++)
        insert(i);

    printf("Even Number: ");
    printList(even);

    printf("Even Number (R): ");
    print_backward(even_last);

    printf("Odd Number : ");
    printList(odd);

    printf("Odd Number (R) : ");
    print_backward(odd_last);

    combine();

    printf("Combined List :\n");
    printList(list);

    printf("Combined List (R):\n");
    print_backward(list_last);

    return 0;
}
