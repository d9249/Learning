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

//sortList() will sort the given list in ascending order  
void sortList() {
	struct node *current = NULL, *index = NULL;
	int temp;
	//Check whether list is empty  
	if (head == NULL) {
		return;
	}
	else {
		//Current will point to head  
		for (current = head; current->next != NULL; current = current->next) {
			//Index will point to node next to current  
			for (index = current->next; index != NULL; index = index->next) {
				//If current's data is greater than index's data, swap the data of current and index  
				if (current->data > index->data) {
					temp = current->data;
					current->data = index->data;
					index->data = temp;
				}
			}
		}
	}
}

//display() will print out the nodes of the list  
void display() {
	//Node current will point to head  
	struct node *current = head;
	if (head == NULL) {
		printf("List is empty\n");
		return;
	}
	while (current != NULL) {
		//Prints each node by incrementing pointer.  
		printf("%d ", current->data);
		current = current->next;
	}
	printf("\n");
}

int main()
{
	//Add nodes to the list  
	addNode(7);
	addNode(1);
	addNode(4);
	addNode(5);
	addNode(2);

	//Displaying original list  
	printf("Original list: \n");
	display();

	//Sorting list  
	sortList();

	//Displaying sorted list  
	printf("Sorted list: \n");
	display();

	return 0;
}