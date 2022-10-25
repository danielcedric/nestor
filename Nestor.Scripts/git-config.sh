# Aliases for git
# Source : https://git.wiki.kernel.org/index.php/Aliases

# git checkout alias
git config --global alias.co checkout

# git branch alias
git config --global alias.br branch

# git status alias
git config --global alias.st 'status -sb'

# display our commits as single lines
git config --global alias.ll 'log --oneline'

# display the last commit: git last
git config --global alias.last 'log -1 HEAD --stat'

# commit : git cm "feat(service): add a new cool feature"
git config --global alias.cm 'commit -m'

# git remote : git rv
git config --global alias.rv 'remote -v'

# git diff (displays differences between files in different commits or between a commit and the working tree) : git d 33559c5 ca1494d file1
git config --global alias.d 'diff'

# git search commits: git se test2
git config --global alias.se '!git rev-list --all | xargs git grep -F'

# autocorrect aliases
git config --global help.autocorrect 20

# make VS Code as my default Diff Tool
git config --global diff.tool vscode
git config --global difftool.vscode.cmd 'code --wait --diff $LOCAL $REMOTE'

# make VS Code your default Merge Tool
git config --global merge.tool vscode
git config --global mergetool.vscode.cmd 'code --wait $MERGED'
