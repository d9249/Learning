#include <stdio.h>
#include<stdlib.h>
struct node
{
    int data;
    struct node *next;
};

struct node *head = NULL;

void addNode(int data)
{
    struct node *newNode = (struct node*)malloc(sizeof(struct node));
    newNode->data = data;
    newNode->next = NULL;

    if(head == NULL)
    {
        head = newNode;
    }
    else
    {
        struct node *current = head;
        while(current->next != NULL)
        {
            current = current->next;
        }
        current->next = newNode;
    }
}

void swapLastWithFirst()
{
    struct node *current = head, *temp = NULL, *index = NULL;

    if(head == NULL)
    {
        return;
    }
    else
    {
        while(current->next != NULL)
        {
            index = current;
            current = current->next;
        }

        if(head == current)
        {
            return;
        }
        else if(head->next == current)
        {
            temp = head;
            head = current;
            head->next = temp;
            temp->next = NULL;
        }
        else
        {
            temp = head;
            head = current;
            head->next = temp->next;
            index->next = temp;
            temp->next = NULL;
        }
    }
}

void display()
{
    struct node *current = head;

    if(head == NULL)
    {
        printf("Empty List!! \n ");
        return;
    }
    while(current != NULL)
    {
        printf("%d ", current->data);
        current = current->next;
    }
    printf("\n");
}

int main()
{
    addNode(1);
    addNode(2);
    addNode(3);
    addNode(4);
    addNode(5);
    printf("Originals list : \n");
    display();
    swapLastWithFirst();

    printf("List after swapping the first node with last : \n");
    display();

    return 0;
}
