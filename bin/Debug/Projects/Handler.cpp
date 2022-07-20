#include<string>
#include "Handler.h"
using namespace std;

string name;
string get_name() {
    return name;
}

string set_name(string name) {
    this->name = name;
}