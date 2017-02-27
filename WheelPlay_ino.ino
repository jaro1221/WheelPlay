const int XPin = A0;
const int RxPin = A0;
const int RyPin = A0;
const int RzPin = A0;

void setup() {
  
  Serial.begin(9600);
}

void loop() {

  // print the results to the serial monitor:
  Serial.print(analogRead(XPin));
  Serial.print(";");
  Serial.print(analogRead(RxPin));
  Serial.print(";");
  Serial.print(analogRead(RyPin));
  Serial.print(";");
  Serial.print(analogRead(XPin));
  Serial.println("");
  
  delay(10);
}
