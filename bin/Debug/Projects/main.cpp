#include<iostream>
#include "Handler.h"
using namespace std;
int main() {
	Handler handler = new Handler("default");
	handler.set_name("new name");
	cout << handler.get_name() << endl;
    for(int i = 0; i < 5; i++) {
        cout << i << endl;
    }
}