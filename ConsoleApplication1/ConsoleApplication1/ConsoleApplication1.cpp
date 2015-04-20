// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#if defined(_WIN32)
#define GLFW_EXPOSE_NATIVE_WIN32
#define GLFW_EXPOSE_NATIVE_WGL
#define OVR_OS_WIN32
#elif defined(__APPLE__)
#define GLFW_EXPOSE_NATIVE_COCOA
#define GLFW_EXPOSE_NATIVE_NSGL
#define OVR_OS_MAC
#elif defined(__linux__)
#define GLFW_EXPOSE_NATIVE_X11
#define GLFW_EXPOSE_NATIVE_GLX
#define OVR_OS_LINUX
#endif
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>
#include <OVR_CAPI_GL.h>
#include "stdafx.h"
#include <iostream>

using namespace std;

void getMonitorData(){
	int count;
	GLFWmonitor** monitors = glfwGetMonitors(&count);
	printf("\nFound %d monitors:\n", count);
	
	for (int i = 0; i < count; ++i)
	{
		GLFWmonitor* pMonitor = monitors[i];
		if (pMonitor == NULL)
			continue;
		printf("  Monitor %d:\n", i);

		/// Monitor name
		const char* pName = glfwGetMonitorName(pMonitor);
		printf("    Name: %s\n", pName);

		/// Monitor Physical Size
		int widthMM, heightMM;
		glfwGetMonitorPhysicalSize(pMonitor, &widthMM, &heightMM);
		printf("    physical size: %d x %d mm\n", widthMM, heightMM);

		const GLFWvidmode* mode = glfwGetVideoMode(pMonitor);
		cout << mode;
	}
}

//DELETE THIS just to test something
static void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
		glfwSetWindowShouldClose(window, GL_TRUE);
}

int buildOpenGLDemo()
{
	getMonitorData();

	// Initializes LibOVR, and the Rift
	ovr_Initialize();
	ovrHmd hmd = ovrHmd_Create(0);

	/* Initialize the GLFW library */
	if (!glfwInit())
		return -1;

	printf("\n HMD Product name is %s \n", hmd->ProductName);
	printf("\n HMD Display name is %s \n", hmd->DisplayDeviceName);
	int hRes = hmd->Resolution.h;
	int wRes = hmd->Resolution.w;
	printf("\n HMD Resultion Height is %i and Width is %i \n", hRes, wRes);
	//cout << hmd->DisplayDeviceName;

	int i, count;
	GLFWmonitor** monitors = glfwGetMonitors(&count);
	for (i = 0; i < count; i++)
	{
		if (strcmp(glfwGetWin32Monitor(monitors[i]), hmd->DisplayDeviceName) == 0)
		{
			const GLFWvidmode* mode = glfwGetVideoMode(monitors[i]);
			glfwWindowHint(GLFW_RED_BITS, mode->redBits);
			glfwWindowHint(GLFW_GREEN_BITS, mode->greenBits);
			glfwWindowHint(GLFW_BLUE_BITS, mode->blueBits);
			glfwWindowHint(GLFW_REFRESH_RATE, mode->refreshRate);
			//Oculus Rift resolution is 960 x 1080 per eye so a total of 1920 x 2160 ? only does 1 eye ... so 4320?
			GLFWwindow* window2 = glfwCreateWindow(mode->width, mode->height, "My Title", NULL, NULL);
			glfwSetWindowPos(window2, mode->width, 0);
			//ovrHmd_AttachToWindow(hmd, window2, NULL, NULL);

			//Configure Stereo settings
			//ovrSizei recommendTex0Size = ovrHmd_GetFovTextureSize(hmd, ovrEye_Left, hmd->DefaultEyeFov[0], 1.0f);
			//ovrSizei recommendTex1Size = ovrHmd_GetFovTextureSize(hmd, ovrEye_Right, hmd->DefaultEyeFov[1], 1.0f);

			//ovrSizei renderTargetSize;
			//renderTargetSize.w = recommendTex0Size.w + recommendTex1Size.w;
			//renderTargetSize.h = max(recommendTex0Size.h, recommendTex1Size.h);

			//const int eyeRenderMultisample = 1;
			//pRendertargetTexture = pRender->CreateTexture(Texture_RGBA | Texture_RenderTarget | eyeRenderMultisample, renderTargetSize.w, renderTargetSize.h, NULL);

			//renderTargetSize.w = pRendertargetTexture->GetWidth();
			//renderTargetSize.h = pRendertargetTexture->GetHeight();

			//Configure OpenGL
			//ovrGLConfig cfg;
			//cfg.OGL.Header.API = ovrRenderAPI_OpenGL;
			//cfg.OGL.Header.RTSize = ovrSizei(hmd->Resolution.w, hmd->Resolution.h);
			//cfg.OGL.Header.Multisample = backBufferMultisample;
			//cfg.OGL.Window = window2;
			//cfg.OGL.DC = dc;

			//ovrBool result = ovrHmd_ConfigureRendering(hmd, &cfg.Config, distortionCaps, eyesFov, EyeRenderDesc);
			// ----------

			glfwMakeContextCurrent(window2);
			glfwSwapInterval(1);
			glfwSetKeyCallback(window2, key_callback);
			while (!glfwWindowShouldClose(window2))
			{
				float ratio;
				int width, height;
				glfwGetFramebufferSize(window2, &width, &height);
				ratio = width / (float)height;
				glViewport(0, 0, width, height);
				glClear(GL_COLOR_BUFFER_BIT);
				glMatrixMode(GL_PROJECTION);
				glLoadIdentity();
				glOrtho(-ratio, ratio, -1.f, 1.f, 1.f, -1.f);
				glMatrixMode(GL_MODELVIEW);
				glLoadIdentity();
				glRotatef((float)glfwGetTime() * 50.f, 0.f, 0.f, 1.f);
				glBegin(GL_TRIANGLES);
				glColor3f(1.f, 0.f, 0.f);
				glVertex3f(-0.6f, -0.4f, 0.f);
				glColor3f(0.f, 1.f, 0.f);
				glVertex3f(0.6f, -0.4f, 0.f);
				glColor3f(0.f, 0.f, 1.f);
				glVertex3f(0.f, 0.6f, 0.f);
				glEnd();
				glfwSwapBuffers(window2);
				glfwPollEvents();
			}

			int tempx, tempy;
			glfwGetWindowPos(window2, &tempx, &tempy);
			cout << "____________\n";
			cout << tempx;
			cout << "//";
			cout << tempy;
			cout << "___________\n";

			int width, height;
			union ovrGLConfig config;
			glfwGetFramebufferSize(window2, &width, &height);
			config.OGL.Header.BackBufferSize.w = width;
			config.OGL.Header.BackBufferSize.h = height;
#if defined(_WIN32)
			config.OGL.Window = glfwGetWin32Window(window2);
#elif defined(__APPLE__)
#elif defined(__linux__)
			config.OGL.Disp = glfwGetX11Display();
#endif


		}
		else
		{
			//int c;
			printf("HMD NOT FOUND\n");
			//cout << "\n Enter a number and press enter to quit out...";
			//cin >> c;
			//cout << "Terminating Libs";
			//glfwTerminate();
			//ovrHmd_Destroy(hmd);
			//ovr_Shutdown();
			//return 0;
		}
	}
}

int main()
{	
	int c;

	//getMonitorData();
	buildOpenGLDemo();


	glfwTerminate();
	ovr_Shutdown();
	return 0;
}

