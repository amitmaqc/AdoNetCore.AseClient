module.exports = {
  generateName: function (userContext, events, done) {
    const names = ["Amit", "Lior", "Dana", "Noa", "Yossi", "Tal", "Roni", "Eli"];
    const randomName = names[Math.floor(Math.random() * names.length)] + "_" + Math.floor(Math.random() * 1000);
    userContext.vars.randomName = randomName;
    return done();
  }
};
