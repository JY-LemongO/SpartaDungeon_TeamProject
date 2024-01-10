using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanTextDungeon
{
    // 데미지를 받을 수 있는 객체에게 상속할 인터페이스
    internal interface IDamagable
    {
        public void GetDamage(float damage);
    }
}
