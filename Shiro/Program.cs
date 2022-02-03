using Shiro;


var c = Clamp(40f,0f,1f);
Console.WriteLine(c);

Console.WriteLine("Hello, World!");

Scene scene = new Scene(784, 784);

Material red = new Material(MaterialType.DIFFUSE, new(0.0f));
red.Kd = new(0.63f, 0.065f, 0.05f);
Material green = new Material(MaterialType.DIFFUSE, new(0.0f));
green.Kd = new(0.14f, 0.45f, 0.091f);
Material white = new Material(MaterialType.DIFFUSE, new(0.0f));
white.Kd = new(0.725f, 0.71f, 0.68f);
Material light = new Material(MaterialType.DIFFUSE, (8.0f * new Vector3f(0.747f + 0.058f, 0.747f + 0.258f, 0.747f) + 15.6f * new Vector3f(0.740f + 0.287f, 0.740f + 0.160f, 0.740f) + 18.4f * new Vector3f(0.737f + 0.642f, 0.737f + 0.159f, 0.737f)));
light.Kd = new(0.65f);

MeshTriangle floor = new("./models/cornellbox/floor.obj", white);
MeshTriangle shortbox = new("./models/cornellbox/shortbox.obj", white);
MeshTriangle tallbox = new("./models/cornellbox/tallbox.obj", white);
//MeshTriangle bunny=new("./models/bunny/bunny.obj", white);
MeshTriangle left = new("./models/cornellbox/left.obj", red);
MeshTriangle right = new("./models/cornellbox/right.obj", green);
MeshTriangle light_ = new("./models/cornellbox/light.obj", light);
//Sphere sphere=new({ 150,100,300 }, 100, white);
//scene.Add(&bunny);
scene.Add(floor);
scene.Add(shortbox);
scene.Add(tallbox);
//scene.Add(&sphere);
scene.Add(left);
scene.Add(right);
scene.Add(light_);

scene.BUlidBVH();

Renderer.Render(scene);




//Scene scene = new Scene(784, 784);
//Material red = new Material(MaterialType.DIFFUSE, new(0.0f));
//red.Kd = new(0.63f, 0.065f, 0.05f);
//Material light = new Material(MaterialType.DIFFUSE, (
//    8.0f * new Vector3f(0.747f + 0.058f, 0.747f + 0.258f, 0.747f)
//    + 15.6f * new Vector3f(0.740f + 0.287f, 0.740f + 0.160f, 0.740f)
//    + 18.4f * new Vector3f(0.737f + 0.642f, 0.737f + 0.159f, 0.737f)));

//light.Kd = new(0.65f);

//MeshTriangle left = new("./models/cornellbox/left.obj", red);
//MeshTriangle light_ = new("./models/cornellbox/light.obj", light);

//scene.Add(left);
//scene.Add(light_);
//scene.BUlidBVH();
//var bvhMain = scene.BVHAccel;
////Ray Ray = new Ray(
////    new(278f, 273f, -800f),
////    new(0.323289126f, 0.323289126f, 0.889363945f)
////    ); 
//Ray Ray = new Ray(
//     new(278f, 273f, -800f),
//     new(0.2804161461f, -0.1385753735f, 0.949822957f)
//     );

////Bound3 Bound3 = new Bound3(new(549.6f, 0, 0), new(556f, 548.8f, 559.2f));

////bvhMain.Intersect(Ray);
////Bound3.IntersectP(Ray);
////bvhMain.Intersect(Ray);
////Console.WriteLine((scene.BVHAccel.root.obj as MeshTriangle).bvh);
//Renderer.Render(scene);




