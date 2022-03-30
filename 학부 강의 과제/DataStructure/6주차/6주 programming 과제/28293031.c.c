#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

struct node
{
	char ssn[20], name[20], dept[10], desg[10];
	int sal;
	long int phno;
	struct node *rlink;
	struct node *llink;
};
typedef struct node* NODE;
NODE first = NULL;
int count = 0;

NODE createEmployeeNode()
{

	NODE employee;
	employee = (NODE)malloc(sizeof(struct node));

	if (employee == NULL)
	{
		printf("Out of Memory\n");
		exit(0);
	}
	employee->rlink = NULL;
	employee->llink = NULL;

	char ssn[20], name[20], dept[10], desg[10];
	int sal;
	long int phno;

	printf("Enter SSN:\t");
	scanf("%s", ssn);
	printf("Enter Name:\t");
	scanf("%s", name);
	printf("Enter Department:\t");
	scanf("%s", dept);
	printf("Enter Designation:\t");
	scanf("%s", desg);
	printf("Enter Salary:\t");
	scanf("%d", &sal);
	printf("Enter Phone Number:\t");
	scanf("%ld", &phno);

	strcpy(employee->ssn, ssn);
	strcpy(employee->name, name);
	strcpy(employee->dept, dept);
	strcpy(employee->desg, desg);

	employee->sal = sal;
	employee->phno = phno;

	count++;
	return employee;
}

NODE insert_front()
{
	NODE temp;
	temp = createEmployeeNode();

	if (first == NULL)
	{
		return temp;
	}

	temp->rlink = first;
	first->llink = temp;

	return temp;
}

NODE insert_end()
{
	NODE cur, temp;
	temp = createEmployeeNode();

	if (first == NULL)
	{
		return temp;
	}
	cur = first;
	while (cur->rlink != NULL)
	{
		cur = cur->rlink;
	}
	cur->rlink = temp;
	temp->llink = cur;

	temp->rlink = NULL;

	return first;



}

NODE del_front()
{
	NODE temp;

	if (first == NULL)
	{
		printf("List is empty!\n");
		return NULL;
	}

	if (first->rlink == NULL)
	{
		printf("The employee record with ssn number %s has been deleted\n", first->ssn);
		free(first);
		count--;
		return NULL;
	}

	temp = first;
	temp->rlink = NULL;
	first->llink = NULL;
	printf("The employee record with ssn number %s has been deleted\n", temp->ssn);
	free(temp);
	count--;

	return first;

}

NODE del_end()
{
	NODE prev, cur;


	if (first == NULL)
	{
		printf("List is empty\n");
		return NULL;
	}

	if (first->rlink == NULL)
	{
		printf("The employee record with ssn number %s has been deleted\n", first->ssn);
		free(first);
		count--;
		return NULL;
	}

	prev = NULL;
	cur = first;

	while (cur->rlink != NULL)
	{
		prev = cur;
		cur = cur->rlink;
	}

	cur->llink = NULL;
	printf("\nThe employee node with ssn %s is deleted\n", cur->ssn);
	free(cur);

	prev->rlink = NULL;
	count--;
	return first;

}

void display()
{
	NODE cur;
	cur = first;

	if (cur == NULL)
	{
		printf("The list is empty\n");
	}
	else
	{

		printf("The contents of the list are\n");
		while (cur != NULL)
		{
			printf("\nSSN: %s\nName: %s\nDepartment: %s\nDesignation: %s\nSalary: %d\nPhone: %ld \n", cur->ssn, cur->name, cur->dept, cur->desg, cur->sal, cur->phno);
			cur = cur->rlink;
		}
		printf("\nThe number of employees: %d", count);
	}
}

void main()
{
	int ch, i, n;
	while (1)
	{
		printf("\t---Menu---\n");
		printf("1. Create DLL of employee\n");
		printf("2. Display\n");
		printf("3. Insert at Front\n");
		printf("4. Insert at End\n");
		printf("5. Delete at front\n");
		printf("6. Delete at end\n");
		printf("7. Exit\n");

		printf("Enter your choice:\t");
		scanf("%d", &ch);

		switch (ch)
		{
		case 1: printf("Enter the number of employees!\n");
			scanf("%d", &n);
			for (i = 0; i<n; i++)
				first = insert_end();
			break;

		case 2: display();
			break;
		case 3: first = insert_front();
			break;
		case 4: first = insert_end();
			break;
		case 5: first = del_front();
			break;
		case 6: first = del_end();
			break;
		case 7: exit(0);
			break;
		default: printf("Invalid choice\n");
			break;

		}


	}
}