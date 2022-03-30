#include<stdio.h>
#include<stdlib.h>
#include<string.h>

struct node* createnode(struct node*);
void display(struct node*);
void search(struct node*);

struct node
{
    char name[10],des[10];
    int age;
    float salary;
    struct node* ptr;
};

int main()
{
    struct node* head;
    int b,i, op;

    head=NULL;

    while(1)
    {

        printf("\n---Enter the value---\n");
        printf("1: Enter the employee details \n");
        printf("2: Display the results \n");
        printf("3: Search an element \n");
        printf("4: Exit \n");
        scanf("%d",&b);

        switch(b)
        {
        case (1):
            printf("\n Enter the number of employee details \n");
            scanf("%d",&i);
            while(i>0)
            {
                head=createnode(head);
                i--;
            }
            break;

        case (2):
            display(head);
            break;

        case (3):
            search(head);
            break;
        case (4):
            exit(0);
        }
    }
}


void display(struct node* head)
{
    if(head==NULL)
    {
        printf("\n The node is yet to be displayed \n");
    }

    else
    {
        while(head!=NULL)
        {
            printf("\n The name of the employee is : %s \n",head->name);
            printf("\n The designation of the employee is : %s \n",head->des);
            printf("\n The salary of the employee is : %f \n",head->salary);
            printf("\nThe age of the employee is : %d \n",head->age);

            head=head->ptr;

        }

    }
}

struct node* createnode(struct node* head)
{

    struct node* newnode;
    newnode=(struct node*)malloc(sizeof (struct node));

    printf("\n Enter the Name : \n");
    scanf("%s",newnode->name);
    printf("\n Enter the Designation : \n");
    scanf("%s",newnode->des);
    printf("\n Enter the Salary : \n");
    scanf("%f",&newnode->salary);
    printf("\n Enter the Age : \n");
    scanf("%d",&newnode->age);

    if(newnode == NULL)
    {
        printf("\n Enter the new node \n");
        newnode->ptr=NULL;
    }

    else
    {
        newnode->ptr=head;

    }
    return newnode;
}


void search(struct node* head)
{
    char ch[10];

    printf("\n Enter the search in string \n");

    scanf("%s",ch);


    while(head!=NULL)
    {
        if(strcmp(ch,head->des)==0)
        {
            printf("\n The element is matched \n");
            printf("\n The salary of the designated member is : %f \n",head->salary);

        }

        else
        {
            printf("\n The element is not matched \n");
        }
        head=head->ptr;
    }
}
