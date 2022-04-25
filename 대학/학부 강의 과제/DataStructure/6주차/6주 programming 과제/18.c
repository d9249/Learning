#include <stdio.h>  

//Represent a node of the doubly linked list  

struct node {
	int data;
	struct node *previous;
	struct node *next;
};

//Represent the head and tail of the doubly linked list  
struct node *head, *tail = NULL;

//addNode() will add a node to the list  
void addNode(int data) {
	//Create a new node  
	struct node *newNode = (struct node*)malloc(sizeof(struct node));
	newNode->data = data;

	//If list is empty  
	if (head == NULL) {
		//Both head and tail will point to newNode  
		head = tail = newNode;
		//head's previous will point to NULL  
		head->previous = NULL;
		//tail's next will point to NULL, as it is the last node of the list  
		tail->next = NULL;
	}
	else {
		//newNode will be added after tail such that tail's next will point to newNode  
		tail->next = newNode;
		//newNode's previous will point to tail  
		newNode->previous = tail;
		//newNode will become new tail  
		tail = newNode;
		//As it is last node, tail's next will point to NULL  
		tail->next = NULL;
	}
}

//MinimumNode() will find out minimum value node in the list  
int minimumNode() {
	//Node current will point to head  
	struct node *current = head;
	int min;

	//Checks if list is empty  
	if (head == NULL) {
		printf("List is empty\n");
		return 0;
	}
	else {
		//Initially, min will store the value of head's data  
		min = head->data;
		while (current != NULL) {
			//If value of min is greater than current's data  
			//Then, replace value of min with current node's data  
			if (min > current->data)
				min = current->data;
			current = current->next;
		}
	}
	return min;
}

//MaximumNode() will find out maximum value node in the list  
int maximumNode() {
	//Node current will point to head  
	struct node *current = head;
	int max;

	//Checks if list is empty  
	if (head == NULL) {
		printf("List is empty\n");
		return 0;
	}
	else {
		//Initially, max will store the value of head's data  
		max = head->data;
		//If value of max is lesser than current's data  
		//Then, replace value of max with current node's data  
		while (current != NULL) {
			if (current->data > max)
				max = current->data;
			current = current->next;
		}
	}
	return max;
}

int main()
{
	addNode(5);
	addNode(7);
	addNode(9);
	addNode(1);
	addNode(2);

	printf("Minimum value node in the list: %d\n", minimumNode());
	printf("Maximum value node in the list: %d", maximumNode());

	return 0;
}