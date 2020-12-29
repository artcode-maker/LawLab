from flask import Flask, render_template, request, escape

from datetime import date
from docxtpl import DocxTemplate

app = Flask(__name__)


def log_request(req:'flask_request', res:str) -> None:
    with open('vsearch.log', 'a') as log:
        print(req.form, req.remote_addr, req.user_agent, res, file=log, sep='|')


@app.route('/search4', methods=['POST'])
def do_search() -> 'html':
    phrase = request.form['phrase']
    letters = request.form['letters']
    title = 'Here are your results:'
    results = str(search4letters(phrase, letters))
    log_request(request, results)
    return render_template( 'results.html',
                            the_title=title,
                            the_phrase=phrase,
                            the_letters=letters,
                            the_results=results,)


@app.route('/')
@app.route('/entry')
def entry_page() -> 'html':
    return render_template('entry.html', the_title="Welcom to search4letters on the web!")


@app.route('/isk')
def entry_isk() -> 'html':
	return render_template('isk.html', the_title="Составим исковое заявление?")



@app.route('/viewlog')
def veiw_the_log() -> 'html':
    contents = []
    with open('vsearch.log') as log:
        for line in log:
            contents.append([])
            for item in line.split('|'):
                contents[-1].append(escape(item))
    titles = ('Form Data', 'Remote_addr', 'User_agent', 'Results')
    return render_template('viewlog.html',
                           the_title='View Log',
                           the_row_titles=titles,
                           the_data=contents,)



def iskovoe(istec, address_istec, telephone_istec, otvetchik, telephone_otvetchik, date_wedding, sud) -> None:
	date_isk = date.today()
	doc = DocxTemplate("sud.docx")
	context = { 'sud' : sud, 'istec' : istec, 'address_istec' : address_istec, 'telephone_istec' : telephone_istec, 'otvetchik' : otvetchik, 'address_otvetchik' : address_otvetchik, 'telephone_otvetchik' : telephone_otvetchik, 'date_wedding' : date_wedding, 'date_isk' : date_isk }
	doc.render(context)
	doc.save("sud_new.docx")



@app.route('/isk4', methods=['POST'])
def do4() -> 'html':
    istec = request.form['istec']
    address_istec = request.form['address_istec']
    telephone_istec = request.form['telephone_istec']
    otvetchik = request.form['otvetchik']
    address_otvetchik = request.form['address_otvetchik']
    telephone_otvetchik = request.form['telephone_otvetchik']
    date_wedding = request.form['date_wedding']
    sud = request.form['sud']
    date_isk = date.today()
    doc = DocxTemplate("sud.docx")
    context = { 'sud' : sud, 'istec' : istec, 'address_istec' : address_istec, 'telephone_istec' : telephone_istec, 'otvetchik' : otvetchik, 'address_otvetchik' : address_otvetchik, 'telephone_otvetchik' : telephone_otvetchik, 'date_wedding' : date_wedding, 'date_isk' : date_isk }
    doc.render(context)
    doc.save("sud_new.docx")
    #iskovoe(istec, address_istec, telephone_istec, otvetchik, telephone_otvetchik, date_wedding, sud)
    title = 'Исковое заявление составлено!'
    return render_template('resultsIsk.html',
                            the_title=title,)



if __name__ == '__main__':
    app.run(debug=True)