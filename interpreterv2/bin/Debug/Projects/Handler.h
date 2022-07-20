#include<string>
using namespace std;
class Handler {
    public:
        Handler(string name);
        string get_name();
        void set_name(string name);
    private:
        string name;
}