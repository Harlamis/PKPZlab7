import tkinter as tk
from tkinter import ttk
from datetime import datetime, timedelta

def calculate_procedure_times():
    try:
        start_time_str = entry_start_time.get()
        next_time_str = entry_next_time.get()
        
        count_str = entry_total_count.get()
        if not count_str:
            raise ValueError("Кількість процедур не може бути порожньою.")
            
        total_count = int(count_str)

        if total_count <= 0:
            raise ValueError("Кількість процедур має бути > 0.")

        start_time = datetime.strptime(start_time_str, "%H:%M")
        next_time = datetime.strptime(next_time_str, "%H:%M")

        if next_time <= start_time:
            next_time += timedelta(days=1)

        interval = next_time - start_time

        procedure_times = []
        current_time = start_time
        
        for i in range(total_count):
            procedure_times.append(current_time.strftime("%H:%M"))
            current_time += interval

        listbox_results.delete(0, tk.END)
        
        for time_str in procedure_times:
            listbox_results.insert(tk.END, time_str)

    except ValueError as e:
        listbox_results.delete(0, tk.END)
        if "time data" in str(e):
            listbox_results.insert(tk.END, "Помилка: Невірний формат часу (введіть HH:MM)")
        elif "invalid literal for int()" in str(e):
            listbox_results.insert(tk.END, "Помилка: Кількість має бути цілим числом")
        else:
            listbox_results.insert(tk.END, f"Помилка: {e}")
    except Exception as e:
        listbox_results.delete(0, tk.END)
        listbox_results.insert(tk.END, f"Виникла невідома помилка: {e}")

root = tk.Tk()
root.title("Планувальник Медичних Процедур")
root.geometry("350x400")

main_frame = ttk.Frame(root, padding="10")
main_frame.pack(fill=tk.BOTH, expand=True)


label_start = ttk.Label(main_frame, text="Час першої процедури (HH:MM):")
label_start.pack(pady=5)
entry_start_time = ttk.Entry(main_frame, width=20)
entry_start_time.pack()
entry_start_time.insert(0, "08:00")

label_next = ttk.Label(main_frame, text="Час наступної процедури (HH:MM):")
label_next.pack(pady=5)
entry_next_time = ttk.Entry(main_frame, width=20)
entry_next_time.pack()
entry_next_time.insert(0, "08:45")

label_count = ttk.Label(main_frame, text="Загальна кількість процедур:")
label_count.pack(pady=5)
entry_total_count = ttk.Entry(main_frame, width=20)
entry_total_count.pack()
entry_total_count.insert(0, "4")

calculate_button = ttk.Button(main_frame, text="Розрахувати", command=calculate_procedure_times)
calculate_button.pack(pady=15)

label_results = ttk.Label(main_frame, text="Список часу процедур:")
label_results.pack(pady=5)

results_frame = ttk.Frame(main_frame)
results_frame.pack(fill=tk.BOTH, expand=True)

scrollbar = ttk.Scrollbar(results_frame, orient=tk.VERTICAL)
listbox_results = tk.Listbox(results_frame, yscrollcommand=scrollbar.set, height=10)

scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
listbox_results.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

scrollbar.config(command=listbox_results.yview)

root.mainloop()