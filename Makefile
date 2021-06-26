program.exe : cpu_temp.cs
	mcs $^ -out:$@

clean:
	rm program.exe

install: program.exe
	cp program.exe ~/bin/cpu_temp.exe